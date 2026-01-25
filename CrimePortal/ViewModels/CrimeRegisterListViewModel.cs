using System;

namespace CrimePortal.ViewModels
{
    public class CrimeRegisterListViewModel
    {

        public int CrimeRegisterId { get; set; }   // ← ★ सर्वात महत्त्वाची ओळ ★
        public int Year { get; set; }
        public string CaseNumber { get; set; }
        public DateTime CrimeDate { get; set; }
        public string AccusedName { get; set; }
        public string PoliceStation { get; set; }

        public string SeizedItems { get; set; }     // जप्त मुद्देमाल वर्णन
        public decimal SeizedValue { get; set; }    // मुद्देमालाची एकूण किंमत

        public string? CarrierName { get; set; }


    }
}
