namespace CrimePortal.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
   
        public class CrimeRegisterViewModel
        {
            // Primary Case Information
            [Required]
            [Display(Name = "वर्ष")]
            public int Year { get; set; }

            [Required]
            [Display(Name = "गुन्हा क्रमांक")]
            public string CaseNumber { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "गुन्हा दिनांक")]
            public DateTime CrimeDate { get; set; }

            [DataType(DataType.Time)]
            [Display(Name = "गुन्ह्याची वेळ (पासून)")]
            public TimeSpan? TimeFrom { get; set; }

            [DataType(DataType.Time)]
            [Display(Name = "वेळ (पर्यंत)")]
            public TimeSpan? TimeTo { get; set; }

            // Accused Details
            public AccusedViewModel Accused { get; set; } = new AccusedViewModel();

            // Panch Witnesses
            public List<PanchaViewModel> Panchas { get; set; } = new List<PanchaViewModel>();

            // Place of Incident
            [Display(Name = "घटनास्थळ")]
            public string PlaceOfIncident { get; set; }

            // Police/Complainant/Investigating Officer
            [Display(Name = "पो. स्टे.")]
            public string PoliceStation { get; set; }

            [Display(Name = "फिर्यादी")]
            public string Complainant { get; set; }

            [Display(Name = "तपास अधिकाऱ्याचे नाव")]
            public string InvestigatingOfficerName { get; set; }

            [Display(Name = "तपास अधिकाऱ्याचे पद")]
            public string InvestigatingOfficerRank { get; set; }

            // Seized Items / Sections
            public List<SectionViewModel> Sections { get; set; } = new List<SectionViewModel>();

            [Display(Name = "गुन्ह्याचा प्रकार / Act")]
            public string Act { get; set; }
       }

        // Nested Models
        public class AccusedViewModel
        {
            [Display(Name = "आरोपीचे नाव")]
            public string Name { get; set; }

            [Display(Name = "वय")]
            public int? Age { get; set; }

            [Display(Name = "लिंग")]
            public string Gender { get; set; }

            [Display(Name = "नातेवाईकाचे नाव")]
            public string RelativeName { get; set; }

            [Display(Name = "पत्ता")]
            public string Address { get; set; }
        }

        public class PanchaViewModel
        {
            [Display(Name = "पंचाचे नाव")]
            public string Name { get; set; }

            [Display(Name = "वय")]
            public int? Age { get; set; }

            [Display(Name = "पत्ता")]
            public string Address { get; set; }
        }

        public class SectionViewModel
        {
           

            [Display(Name = "Section")]
            public string Section { get; set; }

            [Display(Name = "वर्णन")]
            public string Description { get; set; }

            [Display(Name = "प्रमाण")]
            public string Quantity { get; set; }

            [Display(Name = "अंदाजे किंमत")]
            public string ApproxValue { get; set; }
        }

}
