using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimePortal.Models
{
    public class SeizedItem
    {
        [Key]
        public int SeizedItemId { get; set; }

        public string? PropertyClass { get; set; }
        public string? PropertyType { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ApproxValue { get; set; }

        public int CrimeRegisterId { get; set; }

        [ForeignKey(nameof(CrimeRegisterId))]
        public CrimeRegister? CrimeRegister { get; set; }
    }
}
