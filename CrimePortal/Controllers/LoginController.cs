using Microsoft.AspNetCore.Mvc;

namespace CrimePortal.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
