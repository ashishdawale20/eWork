using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CrimePortal.Models
{

    [Index(nameof(Year), nameof(CaseNumber), IsUnique = true)]
    public class CrimeRegister
    {
        public int CrimeRegisterId { get; set; }
        public int Year { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public DateTime CrimeDate { get; set; } = DateTime.Today;
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }

        public string? Complainant { get; set; }
        public string? AccusedName { get; set; }
        public int? AccusedAge { get; set; }
        public string? AccusedGender { get; set; }
        public string? AccusedRelativeName { get; set; }
        public string? AccusedAddress { get; set; }

        public string? PlaceOfIncident { get; set; }
        public string? Act { get; set; }
        public string? Court { get; set; }

        public string? PoliceStation { get; set; }
        public string? InvestigatingOfficerName { get; set; }
        public string? InvestigatingOfficerRank { get; set; }

        public int UserId { get; set; }
        public string? OfficerRank { get; set; }
        public string? OfficeName { get; set; }
        public string? OfficeAddress { get; set; }
        public string? District { get; set; }
        public string? Division { get; set; }

        public string? CarrierName { get; set; }
        public int? SampleNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalSeizedValue { get; set; }

        // Relationships (initialized to avoid null reference)
        public List<Pancha> Panchas { get; set; } = new();
        public List<SeizedItem> SeizedItems { get; set; } = new();
        public List<Sample> Samples { get; set; } = new();

        public decimal? HandBhattiLiters { get; set; }
        public decimal? MohasavaLiters { get; set; }

        public decimal? DeshiLiquorLiters { get; set; }
        public decimal? ForeignLiquorLiters { get; set; }
        public decimal? BeerLiters { get; set; }

        public string? PersonWord { get; set; }     // इसम / महिला
        public string? PersonToWord { get; set; }   // इसमास / महिलेस

        public string? ProperWord { get; set; }   // हा / ही

    }
}
