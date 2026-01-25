using System.ComponentModel.DataAnnotations;

namespace CrimePortal.Models
{
    public class OfficeInfo
    {
        [Key]
        public int OfficeId { get; set; }

        public int UserId { get; set; }      // ⭐ MUST HAVE

        public string Post { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
    }
}
