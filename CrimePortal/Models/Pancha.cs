using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimePortal.Models
{
    public class Pancha
    {
        [Key]
        public int PanchaId { get; set; }

        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }

        // ✅ Proper Foreign Key relation
        public int CrimeRegisterId { get; set; }

        [ForeignKey(nameof(CrimeRegisterId))]
        public CrimeRegister? CrimeRegister { get; set; }
    }
}
