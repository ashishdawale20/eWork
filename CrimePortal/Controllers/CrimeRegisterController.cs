using CrimePortal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CrimePortal.Controllers
{
    public class CrimeRegisterController : Controller
    {
        public IActionResult Index()
        {
            CrimeRegisterViewModel crimeRegisterViewModel = new CrimeRegisterViewModel();
            crimeRegisterViewModel.Panchas = new List<PanchaViewModel>
            {
                new PanchaViewModel {Name = "Test1" },
                new PanchaViewModel { Name = "Test2" }
            };
            return View(crimeRegisterViewModel);
        }
    }
}
