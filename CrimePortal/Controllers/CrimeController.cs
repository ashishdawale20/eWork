using CrimePortal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CrimePortal.Controllers
{
    public class CrimeController : Controller
    {
        public IActionResult Index()
        {
            List<Crime> crimes = new List<Crime>
            {
                new Crime{}
            };
            
            return View(crimes);
        }
    }
}
