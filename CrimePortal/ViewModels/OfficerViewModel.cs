namespace CrimePortal.ViewModels
{
    public class OfficerViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int OfficerId { get; set; }   // 🔑 MUST
        public string OfficerName { get; set; }
        public string OfficerRank { get; set; }
        public string OfficeName { get; set; }       // जर तू binding करत असशील
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
    }

}
