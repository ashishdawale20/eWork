using System.ComponentModel.DataAnnotations;

namespace CrimePortal.Models
{
    public class Officer
    {
        [Key]
        public int OfficerId { get; set; }     // PK

        public int UserId { get; set; }        // FK to User

        public string OfficerName { get; set; }
        public string OfficerRank { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
    }
}
