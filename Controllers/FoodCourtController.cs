using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lakhani.Controllers
{
    public class FoodCourtController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public FoodCourtController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult FoodCourt()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            return View();
        }

        [HttpPost]
        public IActionResult FoodCourt(FoodCourts food, IFormFile Image)
        {
            if (Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string path = Path.Combine(env.WebRootPath, "ShopImage/", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                food.ImagePath = fileName;
                this.context.FoodCourts.Add(food);
                this.context.SaveChanges();
            }
            return RedirectToAction("ViewFoodCourt");
        }

        public IActionResult ViewFoodCourt()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var Food = this.context.FoodCourts.ToList();
            return View(Food);
        }

        public IActionResult Edit(int Id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var food = this.context.FoodCourts.FirstOrDefault(x => x.Id == Id);
            return View(food);
        }
        [HttpPost]
        public IActionResult Edit(FoodCourts food, IFormFile ImagePath)
        {
            var old_data = this.context.FoodCourts.FirstOrDefault(x => x.Id == food.Id);
            if (old_data == null)
            {
                return NotFound();
            }

            old_data.CounterName = food.CounterName;
            old_data.ItemName = food.ItemName;
            old_data.Price = food.Price;

            if (ImagePath != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + ImagePath.FileName;
                string path = Path.Combine(env.WebRootPath, "ShopImage/", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImagePath.CopyTo(stream);
                }
                old_data.ImagePath = fileName;
            }
            this.context.SaveChanges();
            return RedirectToAction("ViewFoodCourt");
        }

        public IActionResult Delete(int Id)
        {
            var del = this.context.FoodCourts.FirstOrDefault(x => x.Id == Id);
            this.context.FoodCourts.Remove(del);
            this.context.SaveChanges();
            return RedirectToAction("ViewFoodCourt");
        }
    }
}
