namespace CrimePortal.ViewModels
{
    public class OfficeViewModel
    {
        public int OfficeId { get; set; }

        public int UserId { get; set; }
        public string OfficeName { get; set; }    // जर लागणार असेल
        public string Post { get; set; }          // जर लागणार असेल
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
    }

}
