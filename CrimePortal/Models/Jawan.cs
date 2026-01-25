using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimePortal.Models
{
    public class Jawan
    {
        [Key]
        public int JawanId { get; set; }

        public int UserId { get; set; }          // ⭐ MUST HAVE

        [Required]
        public string? JawanName { get; set; }

        [NotMapped]
        public string? Role { get; set; }

        // Rank removed because you don’t need it

        public string? OfficeName { get; set; }
        public string? OfficeAddress { get; set; }
        public string? District { get; set; }
        public string? Division { get; set; }
    }
}
