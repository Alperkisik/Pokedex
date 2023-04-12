using Microsoft.AspNetCore.Mvc;
using Pokedex.Models;
using Pokedex.Models.Data;

namespace Pokedex.Controllers
{
    public class AdminController : Controller
    {
        private readonly PokedexContext db;

        public AdminController(PokedexContext context)
        {
            db = context;
        }

        public IActionResult Login()
        {
            //HttpContext.Session.SetString("Username", "Alper");
            //return RedirectToAction("Index", "Admin");

            return View("Login");
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            bool success = true;
            User user = db.Users.Where(i => i.Username == Username).FirstOrDefault();

            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found.";
                success = false;
            }

            if (user.Auth != "admin")
            {
                ViewBag.ErrorMessage = "User is not an admin.";
                success = false;
            }

            if (user.Password != Password)
            {
                ViewBag.ErrorMessage = "Wrong Password.";
                success = false;
            }

            if (success)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserType", user.Auth);
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
