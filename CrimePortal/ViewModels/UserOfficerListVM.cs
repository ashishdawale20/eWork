namespace CrimePortal.ViewModels
{
    public class UserOfficerListVM
    {
        public int UserId { get; set; }

        // User table fields
        public string UserName { get; set; }
        
        public string UserType { get; set; }

        public bool IsActive { get; set; }

        // Office table fields
        public string Post { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Division { get; set; }

        public List<UserOfficerListVM> Officers { get; set; } = new List<UserOfficerListVM>();
    }
}
