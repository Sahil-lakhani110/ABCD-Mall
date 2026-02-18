using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Scripting;

namespace Lakhani.Controllers
{
    public class SignupController : Controller
    {
        private readonly AppDbContext context;

        public SignupController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(Signup user)
        {
            if (ModelState.IsValid)
            {
                var data = this.context.Signup.FirstOrDefault(x => x.Email == user.Email);
                if (data != null)
                {
                    TempData["Error"] = "Email is Already Exist..!";
                    return View(user);
                }
                this.context.Signup.Add(user);
                this.context.SaveChanges();
            }
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login user)
        {
            var VerifyUser = this.context.Signup.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (VerifyUser == null)
            {
                TempData["Error"] = "Invalid Credentials.!";
                return View(user);
            }
            if (VerifyUser.Role == "Admin")
            {
                TempData["Success"] = "Login Successfully.!";
                HttpContext.Session.SetString("Admin", VerifyUser.Username);
                HttpContext.Session.SetInt32("AdminId", VerifyUser.Id);
                return RedirectToAction("Index", "Admin");
            }

            TempData["Success"] = "Login Successfully.!";
            HttpContext.Session.SetString("User", VerifyUser.Username);
            HttpContext.Session.SetInt32("UserId", VerifyUser.Id);
            return RedirectToAction("Index", "Home");
        }

    }
}
