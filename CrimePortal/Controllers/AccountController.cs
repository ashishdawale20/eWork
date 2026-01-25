using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using CrimePortal.Data;
using CrimePortal.Models;
using System.Threading.Tasks;
using System.Linq;
using CrimePortal.Helpers;
using CrimePortal.ViewModels;

namespace CrimePortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }

            // ✅ User only by username
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // 🚫 Account inactive check
            if (!user.IsActive)
                if (user.UserType != "Admin" && !user.IsActive)
                {
                ViewBag.Error = "हे अकाऊंट तात्पुरते बंद आहे";
                return View();
            }

            // 🔒 PASSWORD VERIFY (HASH)
            if (!PasswordHelper.Verify(user.PasswordHash, password))
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // ✅ LOGIN SUCCESS
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.UserType)
    };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

            if (user.UserType == "Admin")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "CrimeRegister");
        }





        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return Content("You are not authorized to access this page.");
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Current logged-in user
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId);

            if (user == null)
                return RedirectToAction("Login");

            // 🔒 HASHED password verify
            if (!PasswordHelper.Verify(user.PasswordHash, model.CurrentPassword))
            {
                ModelState.AddModelError("", "Current password is incorrect");
                return View(model);
            }

            // 🔐 Save NEW hashed password
            user.PasswordHash = PasswordHelper.Hash(model.NewPassword);
            _context.SaveChanges();

            // 🔹 Redirect to main page after password change
            if (user.UserType == "Admin")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "CrimeRegister");
        }


    }
}
