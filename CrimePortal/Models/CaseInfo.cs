using System;

namespace CrimePortal.Models
{
    public class CaseInfo
    {
        public int Id { get; set; }

        // Template placeholders के अनुसार properties
        public string CaseNumber { get; set; }
        public DateTime CaseDate { get; set; }
        public string AccuserName { get; set; }
        public string AccusedName { get; set; }
        public string Address { get; set; }

        // Optional fields (अगर ज़रूरत पड़े)
        public int AccuserAge { get; set; }
        public string LawSection { get; set; }
        public string OfficerName { get; set; }
        public string CourtInvoice { get; set; }
    }
}
