using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace Lakhani.Controllers
{
    public class MovieController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public MovieController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public IActionResult Movie()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Movie(Movies movies, IFormFile Image)
        {
            if (Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string path = Path.Combine(env.WebRootPath, "ShopImage/", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                movies.Image = fileName;

                movies.AvailableSeats = movies.TotalSeats;

                this.context.Movies.Add(movies);
                this.context.SaveChanges();
            }
            return RedirectToAction("ViewMovie");
        }

        public IActionResult ViewMovie()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var movies = this.context.Movies.ToList();
            return View(movies);
        }

        public IActionResult Edit(int Id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var mov = this.context.Movies.FirstOrDefault(x => x.Id == Id);
            return View(mov);
        }

        [HttpPost]
        public IActionResult Edit(Movies mov, IFormFile Image)
        {
            var old_data = this.context.Movies.FirstOrDefault(x => x.Id == mov.Id);
            if (old_data == null)
            {
                return NotFound();
            }

            old_data.MovieName = mov.MovieName;
            old_data.ShowTime = mov.ShowTime;
            old_data.TotalSeats = mov.TotalSeats;
            old_data.AvailableSeats = mov.AvailableSeats;

            if (Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string path = Path.Combine(env.WebRootPath, "ShopImage/", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                old_data.Image = fileName;
            }
            this.context.SaveChanges();
            return RedirectToAction("ViewMovie");
        }

        // ================== DELETE ==================
        public IActionResult Delete(int Id)
        {
            var del = this.context.Movies.FirstOrDefault(x => x.Id == Id);
            this.context.Movies.Remove(del);
            this.context.SaveChanges();
            return RedirectToAction("ViewMovie");
        }

        public IActionResult Book(int id)
        {
            var movie = context.Movies.FirstOrDefault(x => x.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // ===================== BOOK (POST) =====================
        [HttpPost]
        public IActionResult Book(int MovieId, string UserName, int SeatsBooked)
        {
            var movie = context.Movies.FirstOrDefault(x => x.Id == MovieId);

            if (movie == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(UserName))
            {
                TempData["Error"] = "Please enter your name!";
                return RedirectToAction("Book", new { id = MovieId });
            }

            if (SeatsBooked <= 0)
            {
                TempData["Error"] = "Please select at least 1 seat!";
                return RedirectToAction("Book", new { id = MovieId });
            }

            if (SeatsBooked > movie.AvailableSeats)
            {
                TempData["Error"] = "You cannot book more seats than available!";
                return RedirectToAction("Book", new { id = MovieId });
            }

            decimal ticketPrice = 1000;
            decimal totalAmount = SeatsBooked * ticketPrice;

            // Booking save (Payment se pehle)
            MovieBookings booking = new MovieBookings()
            {
                MovieId = MovieId,
                UserName = UserName,
                SeatsBooked = SeatsBooked,
                BookingDate = DateTime.Now,
                Amount = totalAmount
            };

            context.MovieBookings.Add(booking);
            context.SaveChanges();

            // ✅ Redirect to Payment with booking id
            return RedirectToAction("Payment", new { id = booking.Id });
        }

        // ===================== PAYMENT (GET) =====================
        public IActionResult Payment(int id)
        {
            var booking = context.MovieBookings
                .Include(b => b.Movie)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // ===================== PAYMENT (POST) =====================
        [HttpPost]
        public IActionResult PaymentSuccess(int id, string CardNumber, string Expiry, string CVV)
        {
            var booking = context.MovieBookings
                .Include(b => b.Movie)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            // Simple validations (demo)
            if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 16)
            {
                TempData["PayError"] = "Invalid card number!";
                return RedirectToAction("Payment", new { id });
            }

            if (string.IsNullOrWhiteSpace(CVV) || CVV.Length < 3)
            {
                TempData["PayError"] = "Invalid CVV!";
                return RedirectToAction("Payment", new { id });
            }

            // ✅ Payment Successful - Now reduce seats
            var movie = context.Movies.FirstOrDefault(m => m.Id == booking.MovieId);
            if (movie == null)
                return NotFound();

            if (booking.SeatsBooked > movie.AvailableSeats)
            {
                TempData["PayError"] = "Seats are no longer available!";
                return RedirectToAction("Payment", new { id });
            }

            movie.AvailableSeats -= booking.SeatsBooked;
            context.SaveChanges();

            // ✅ Redirect to confirmation page
            return RedirectToAction("Confirmation", new { id = booking.Id });
        }

        // ===================== CONFIRMATION (GET) =====================
        public IActionResult Confirmation(int id)
        {
            var booking = context.MovieBookings
                .Include(b => b.Movie)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // ===================== DOWNLOAD TICKET (PDF) =====================
        public IActionResult DownloadTicket(int id)
        {
            var booking = context.MovieBookings
                .Include(b => b.Movie)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            // ✅ Generate PDF bytes
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("🎟 Movie Ticket")
                        .FontSize(22)
                        .Bold()
                        .AlignCenter();

                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Booking ID: {booking.Id}").Bold();
                        col.Item().Text($"Name: {booking.UserName}");
                        col.Item().Text($"Movie: {booking.Movie?.MovieName}");
                        col.Item().Text($"Show Time: {booking.Movie?.ShowTime}");
                        col.Item().Text($"Seats Booked: {booking.SeatsBooked}");
                        col.Item().Text($"Amount Paid: Rs {booking.Amount}").Bold();
                        col.Item().Text($"Booking Date: {booking.BookingDate:dd MMM yyyy, hh:mm tt}");

                        col.Item().PaddingTop(15).LineHorizontal(1);

                        col.Item().Text("✅ Please bring this ticket to the counter or show on mobile.")
                            .Italic().FontSize(11).FontColor(Colors.Grey.Darken1);
                    });

                    page.Footer().AlignCenter().Text("ABCD Mall Cinema • Thank you for booking!");
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Ticket_{booking.Id}.pdf");
        }
    }
}
