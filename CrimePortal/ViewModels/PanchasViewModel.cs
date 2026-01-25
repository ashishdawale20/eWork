using CrimePortal.Models;

namespace CrimePortal.ViewModels
{
    public class PanchaViewModel
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }

        public CrimeRegister CrimeRegister { get; set; }
    }
}
