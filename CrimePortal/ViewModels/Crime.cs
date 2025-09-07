using System.ComponentModel.DataAnnotations;

namespace CrimePortal.ViewModels
{
    public class Crime
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "गु र क्र व दिनांक")]
        public string CrimeNoDate { get; set; }

        [Display(Name = "गुन्ह्याचे ठिकाण")]
        public string Place { get; set; }

        [Display(Name = "फिर्यादी")]
        public string Complainant { get; set; }

        [Display(Name = "आरोपी व पत्ता")]
        public string AccusedAddress { get; set; }

        [Display(Name = "अटक तारीख व वेळ")]
        public DateTime? ArrestDate { get; set; }

        [Display(Name = "पंचनामा तारीख व वेळ")]
        public DateTime? PanchnamaDate { get; set; }

        [Display(Name = "जप्त मुद्देमाल थोडक्यात वर्णन")]
        public string SeizedItems { get; set; }

        [Display(Name = "मुद्देमाल किंमत")]
        public decimal? SeizedValue { get; set; }

        [Display(Name = "साक्षिदारांची नावे व पत्ता")]
        public string Witnesses { get; set; }

        [Display(Name = "गुन्हा नोदविणाऱ्या अधिकाऱ्याचे नाव व पद")]
        public string OfficerName { get; set; }
    }
}
