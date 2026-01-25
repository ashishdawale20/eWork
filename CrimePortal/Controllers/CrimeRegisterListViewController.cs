using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrimePortal.Data;
using CrimePortal.ViewModels;
using System;
using System.Linq;
using SkiaSharp;
using System.Security.Claims;

namespace CrimePortal.Controllers
{
    public class CrimeRegisterListViewController : Controller
    {
        private readonly CRDbContext _crimecontext;

        private readonly AppDbContext _appcontext;

        public CrimeRegisterListViewController(CRDbContext crimecontext, AppDbContext appcontext)
        {
            _crimecontext = crimecontext;

            _appcontext = appcontext;
        }

        // ✅ GET: /CrimeRegisterListView
        public IActionResult Index(int page = 1)
        {

            int pageSize = 20;   // 🔢 एक page पर कितने records


            // ✅ Step 1: Username घ्या
            string loginUserName = User.Identity.Name;

            // ✅ Step 2: DB मधून user काढा
            var user = _appcontext.Users
                .FirstOrDefault(u => u.UserName == loginUserName);

            if (user == null)
                return Unauthorized();

            int loginUserId = user.UserId;   // 🔥 INT – DB MATCH


            // 🔢 Total records
            int totalRecords = _crimecontext.CrimeRegisters
                .Count(c => c.UserId == loginUserId);

            // 📄 Total pages
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Step 1: डेटा DB मधून fetch करा (SeizedItems सहित)
            var crimes = _crimecontext.CrimeRegisters
                .Where(c => c.UserId == loginUserId)   // 🔐 user का data
                .Include(c => c.SeizedItems)
                .OrderByDescending(c => c.CrimeDate)
                .Skip((page - 1) * pageSize)           // 📄 pagination
                .Take(pageSize)
                .ToList();  // ⚡️ SQL query इथे execute होते

            // Step 2: LINQ projection (C# level वर)
            var model = crimes.Select(c => new CrimeRegisterListViewModel
            {
                CrimeRegisterId = c.CrimeRegisterId,   // ← ★ इथे मॅप करणे आवश्यक
                Year = c.Year,
                CaseNumber = c.CaseNumber,
                CrimeDate = c.CrimeDate,
                AccusedName = c.AccusedName ?? "—",
                PoliceStation = c.PoliceStation ?? "—",

                CarrierName = c.CarrierName ?? "",

                // ✅ जप्त मुद्देमाल थोडक्यात वर्णन
                SeizedItems = (c.SeizedItems != null && c.SeizedItems.Any())
                    ? string.Join(", ", c.SeizedItems
                        .Where(x => !string.IsNullOrEmpty(x.Description))
                        .OrderBy(x => x.SeizedItemId)
                        .Select(x => x.Description))
                    : "—",

                // ✅ मुद्देमालाची एकूण किंमत
                SeizedValue = (c.SeizedItems != null && c.SeizedItems.Any())
                    ? c.SeizedItems.Sum(x => x.ApproxValue ?? 0)
                    : 0
            })
            .ToList();

            ViewBag.Jawans = _appcontext.Jawans
            .Where(j => j.UserId == loginUserId)
            .Select(j => j.JawanName)
            .Distinct()
            .ToList();


            // 🔥🔥🔥 Pagination ViewBag (यही paste करना था)
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(model);
        }


        public IActionResult Delete(int id)
        {
            var record = _crimecontext.CrimeRegisters.Find(id);

            if (record == null)
                return NotFound();

            _crimecontext.CrimeRegisters.Remove(record);
            _crimecontext.SaveChanges();

            TempData["SuccessMessage"] = $"गुन्हा क्र. {record.CaseNumber} यशस्वीरीत्या डिलीट करण्यात आला.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCarrier(int id, string carrierName)
        {
            if (string.IsNullOrWhiteSpace(carrierName))
                return BadRequest("Carrier name required");

            var record = _crimecontext.CrimeRegisters
                .FirstOrDefault(x => x.CrimeRegisterId == id);

            if (record == null)
                return NotFound();

            record.CarrierName = carrierName;
            _crimecontext.SaveChanges();

            return Ok();
        }





    }
}
