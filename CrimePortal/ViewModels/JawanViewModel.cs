namespace CrimePortal.ViewModels
{
    public class JawanViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int JawanId { get; set; }   // 🔑 MUST
        public int OfficeId { get; set; }           // Optional, User के link के लिए
        public string JawanName { get; set; }     // Name of the jawan
        public string OfficeName { get; set; }    // Office link
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
    }
}
