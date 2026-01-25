using System.Collections.Generic;

namespace CrimePortal.ViewModels
{
    public class UserOfficerViewModel
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        public string Post { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public List<OfficeViewModel> Offices { get; set; } = new List<OfficeViewModel>();

        public List<OfficerViewModel> Officers { get; set; } = new List<OfficerViewModel>();

        // तुम्ही Constables वापरत असाल तर:
        
        public List<JawanViewModel> Jawans { get; set; } = new List<JawanViewModel>();

               // जर तुमच्या view मध्ये 'Office' property वापरली असेल तर:
       }
}
