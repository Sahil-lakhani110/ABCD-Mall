using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Lakhani.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ShopController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Shop()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Shop(Shops shp, IFormFile Image)
        {
            if (Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string path = Path.Combine(env.WebRootPath, "ShopImage/", fileName);

                using(var stream = new FileStream(path, FileMode.Create)) 
                {
                    Image.CopyTo(stream);
                }
                shp.Image = fileName;
                this.context.Shops.Add(shp);
                this.context.SaveChanges();
            }
            return RedirectToAction("ViewShop");
        }

        public IActionResult ViewShop()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var shop = this.context.Shops.ToList();
            return View(shop);
        }

        public IActionResult Edit(int Id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var shop = this.context.Shops.FirstOrDefault(x => x.Id == Id);
            return View(shop);
        }

        [HttpPost]
        public IActionResult Edit(Shops shp, IFormFile Image)
        {
            var old_data = this.context.Shops.FirstOrDefault(x => x.Id == shp.Id);
            if (old_data == null)
            {
                return NotFound();
            }

            old_data.ShopName = shp.ShopName;
            old_data.Category = shp.Category;
            old_data.Description = shp.Description;

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
            return RedirectToAction("ViewShop");
        }

        public IActionResult Delete(int Id) 
        {
            var del = this.context.Shops.FirstOrDefault(x => x.Id == Id);
            this.context.Shops.Remove(del);
            this.context.SaveChanges();
            return RedirectToAction("ViewShop");
        }
    }
}
