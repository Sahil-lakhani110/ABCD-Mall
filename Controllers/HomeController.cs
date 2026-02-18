using Humanizer;
using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lakhani.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            var viewmodel = new ShopIndexViewModel 
            { 
                Shops = this.context.Shops.OrderBy(s => Guid.NewGuid()).Take(12).ToList(),
                Galleries = this.context.Gallerys.ToList()
            };

            return View(viewmodel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Feeback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Feeback(Feedback feedback)
        {
            if (ModelState.IsValid) 
            {
                this.context.Feedbacks.Add(feedback);
                this.context.SaveChanges();
                TempData["SuccessMessage"] = "Feedback Submitted Successfully!";
                return RedirectToAction("Feeback");
            }
            return View(feedback);
        }
        public IActionResult SubmitFeedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                context.Feedbacks.Add(feedback);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Feedback Submitted Successfully!";
            }

            return RedirectToAction("Index");
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contact con)
        {
            if (ModelState.IsValid)
            {
                this.context.Contacts.Add(con);
                this.context.SaveChanges();
                TempData["Message"] = "Thank you! Your message has been sent.";
                return RedirectToAction("Contact");
            }
            return View(con);
        }

        public IActionResult Shop()
        {
            var shop = this.context.Shops.ToList();
            return View(shop);
        }

        public IActionResult FoodCourt()
        {
            var food = this.context.FoodCourts.ToList();
            return View(food);
        }

        public IActionResult Gallery()
        {
            var gallery = this.context.Gallerys.ToList();
            return View(gallery);
        }

        public IActionResult Movies()
        {
            var movie = this.context.Movies.ToList();
            return View(movie);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
