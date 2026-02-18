using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Lakhani.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext context;

        public AdminController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }

            int shop = this.context.Shops.Count();
            ViewBag.ShopCount = shop;

            int food = this.context.FoodCourts.Count();
            ViewBag.FoodCount = food;

            int movie = this.context.Movies.Count();
            ViewBag.MovieCount = movie;

            int gal = this.context.Gallerys.Count();
            ViewBag.GalCount = gal;

            int feed = this.context.Feedbacks.Count();
            ViewBag.feedCount = feed;

            int con = this.context.Contacts.Count();
            ViewBag.conCount = con;

            int book = this.context.MovieBookings.Count();
            ViewBag.BookCount = book;

            return View();
        }

        public IActionResult Logout() 
        {
            HttpContext.Session.Remove("Admin");
            HttpContext.Session.Remove("AdminId");
            return RedirectToAction("Login", "Signup");
        }

        public IActionResult ViewFeedback()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var feed = this.context.Feedbacks.ToList();
            return View(feed);
        }

        public IActionResult ViewContact()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var con = this.context.Contacts.ToList();
            return View(con);
        }

        public IActionResult ViewBookings()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var bookings = context.MovieBookings
        .Include(b => b.Movie)
        .OrderByDescending(b => b.BookingDate)
        .ToList();
            return View(bookings);
        }

        public IActionResult DeleteCon(int Id)
        {
            var del = this.context.Contacts.FirstOrDefault(x => x.Id == Id);
            this.context.Contacts.Remove(del);
            this.context.SaveChanges();
            return RedirectToAction("ViewContact");
        }

        public IActionResult DeleteFeed(int Id)
        {
            var del = this.context.Feedbacks.FirstOrDefault(x => x.FeedbackId == Id);
            this.context.Feedbacks.Remove(del);
            this.context.SaveChanges();
            return RedirectToAction("ViewFeedback");
        }

    }
}
