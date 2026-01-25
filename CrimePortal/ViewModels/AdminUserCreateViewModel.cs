using System.Collections.Generic;

namespace CrimePortal.ViewModels
{
    public class AdminUserCreateViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        public List<OfficeViewModel> Offices { get; set; } = new List<OfficeViewModel>();

        public string Post
        {
            get => Offices.Count > 0 ? Offices[0].Post : string.Empty;
            set { if (Offices.Count == 0) Offices.Add(new OfficeViewModel()); Offices[0].Post = value; }
        }

        public string OfficeName
        {
            get => Offices.Count > 0 ? Offices[0].OfficeName : string.Empty;
            set { if (Offices.Count == 0) Offices.Add(new OfficeViewModel()); Offices[0].OfficeName = value; }
        }

        public string OfficeAddress
        {
            get => Offices.Count > 0 ? Offices[0].OfficeAddress : string.Empty;
            set { if (Offices.Count == 0) Offices.Add(new OfficeViewModel()); Offices[0].OfficeAddress = value; }
        }

        public string District
        {
            get => Offices.Count > 0 ? Offices[0].District : string.Empty;
            set { if (Offices.Count == 0) Offices.Add(new OfficeViewModel()); Offices[0].District = value; }
        }

        public string Division
        {
            get => Offices.Count > 0 ? Offices[0].Division : string.Empty;
            set { if (Offices.Count == 0) Offices.Add(new OfficeViewModel()); Offices[0].Division = value; }
        }

        public List<OfficerViewModel> Officers { get; set; } = new List<OfficerViewModel>();
        public List<JawanViewModel> Jawans { get; set; } = new List<JawanViewModel>();
    }
}
