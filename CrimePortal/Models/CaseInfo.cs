using System.ComponentModel.DataAnnotations;

namespace CrimePortal.Models
{
    public class CaseInfo
    {
        [Required]
        public string CaseNumber { get; set; }
        [Required]
        public string CaseDate { get; set; }
        [Required]
        public string AccuserName { get; set; }
        [Required]
        public string AccusedName { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
