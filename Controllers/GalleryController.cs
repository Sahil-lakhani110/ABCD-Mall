using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lakhani.Controllers
{
    public class GalleryController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public GalleryController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Gallery()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Gallery(Gallery gal, IFormFile Image)
        {
            if (Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string path = Path.Combine(env.WebRootPath, "ShopImage/", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                gal.Image = fileName;
                this.context.Gallerys.Add(gal);
                this.context.SaveChanges();
            }
            return RedirectToAction("ViewGallery");
        }

        public IActionResult ViewGallery()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var gal = this.context.Gallerys.ToList();
            return View(gal);
        }

        public IActionResult Edit(int Id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Signup");
            }
            var gal = this.context.Gallerys.FirstOrDefault(x => x.Id == Id);
            return View(gal);
        }

        [HttpPost]
        public IActionResult Edit(Gallery gal, IFormFile Image)
        {
            var old_data = this.context.Gallerys.FirstOrDefault(x => x.Id == gal.Id);
            if (old_data == null)
            {
                return NotFound();
            }

            old_data.Title = gal.Title;

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
            return RedirectToAction("ViewGallery");
        }

        public IActionResult Delete(int Id)
        {
            var del = this.context.Gallerys.FirstOrDefault(x => x.Id == Id);
            this.context.Gallerys.Remove(del);
            this.context.SaveChanges();
            return RedirectToAction("ViewGallery");
        }
    }
}
