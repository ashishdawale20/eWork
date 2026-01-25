using CrimePortal.Models;

namespace CrimePortal.ViewModels
{
    public class SeizedItemViewModel
    {
        public string PropertyClass { get; set; }
        public string PropertyType { get; set; }
        public string Description { get; set; }
        public decimal ApproxValue { get; set; }

        public CrimeRegister CrimeRegister { get; set; }
    }

}
