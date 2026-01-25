using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimePortal.Models
{
    public class Sample
    {
        [Key]
        public int SampleId { get; set; }

        public string? SampleType { get; set; }

        public int CrimeRegisterId { get; set; }

        [ForeignKey(nameof(CrimeRegisterId))]
        public CrimeRegister? CrimeRegister { get; set; }
    }
}
