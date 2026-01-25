using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrimePortal.Data;
using CrimePortal.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CrimePortal.Controllers
{
    public class CrimeController : Controller
    {
        private readonly CRDbContext _context;

        private readonly AppDbContext _appContext;

        public CrimeController(CRDbContext context, AppDbContext appContext)
        {
            _context = context;
            _appContext = appContext;
        }


        // ✅ GET: /Crime/Index
        public IActionResult Index(int page = 1, int pageSize = 15)
        {

            string loginUserName = User.Identity.Name;

            var user = _appContext.Users
       .FirstOrDefault(u => u.UserName == loginUserName);

            if (user == null)
                return Unauthorized();

            int loginUserId = user.UserId;

            // 🔢 Total records (sirf user ke)
            int totalRecords = _context.CrimeRegisters
                .Count(c => c.UserId == loginUserId);


            var crimes = _context.CrimeRegisters
        .Where(c => c.UserId == loginUserId)   // ⭐ MAIN LINE
        .Include(c => c.SeizedItems)
        .Include(c => c.Panchas)
        .OrderByDescending(c => c.CrimeDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(c => new Crime
        {
                    Id = c.CrimeRegisterId,
                    CrimeNoDate = $"{c.CaseNumber}/{c.Year} - {c.CrimeDate:dd-MM-yyyy}",
                    Place = c.PlaceOfIncident ?? "—",
                    Complainant = c.Complainant ?? "—",
                    AccusedAddress = $"{(c.AccusedName ?? "—")} {(c.AccusedAddress ?? "")}".Trim(),

                    ArrestDate = c.TimeFrom.HasValue
                        ? c.CrimeDate.Date.Add(c.TimeFrom.Value)
                        : (DateTime?)null,

                    PanchnamaDate = c.TimeTo.HasValue
                        ? c.CrimeDate.Date.Add(c.TimeFrom.Value)
                        : (DateTime?)null,

                    SeizedItems = c.SeizedItems != null && c.SeizedItems.Any()
                        ? string.Join(", ", c.SeizedItems.Select(s => s.Description))
                        : "—",

                    SeizedValue = c.SeizedItems != null && c.SeizedItems.Any()
                        ? c.SeizedItems.Sum(s => s.ApproxValue ?? 0)
                        : 0,

                    Witnesses = c.Panchas != null && c.Panchas.Any()
                        ? string.Join(", ", c.Panchas.Select(p => $"{p.Name} ({p.Address})"))
                        : "—",

                    OfficerName = $"{(c.InvestigatingOfficerName ?? "—")} ({(c.InvestigatingOfficerRank ?? "")})"
                })
                .ToList();

            // Pagination Info
            ViewBag.Page = page;
            ViewBag.TotalPages = Math.Ceiling((double)_context.CrimeRegisters.Count() / pageSize);

            return View(crimes);
        }


        // ✅ DELETE: /Crime/DeleteSelected
        [HttpPost]
        public async Task<IActionResult> DeleteSelected([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return BadRequest("No IDs received.");

            try
            {
                var crimesToDelete = await _context.CrimeRegisters
                    .Include(c => c.Panchas)
                    .Include(c => c.SeizedItems)
                    .Include(c => c.Samples)
                    .Where(c => ids.Contains(c.CrimeRegisterId))
                    .ToListAsync();

                if (!crimesToDelete.Any())
                    return NotFound("No records found to delete.");

                // संबंधित डेटा delete करा
                foreach (var crime in crimesToDelete)
                {
                    _context.Panchas.RemoveRange(crime.Panchas);
                    _context.SeizedItems.RemoveRange(crime.SeizedItems);
                    _context.Samples.RemoveRange(crime.Samples);
                }

                _context.CrimeRegisters.RemoveRange(crimesToDelete);
                await _context.SaveChangesAsync();

                return Ok("रेकॉर्ड डिलीट झाले.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error deleting records: " + ex.Message);
            }
        }

        // ✅ EDIT: /Crime/Edit/5 — Edit बटणावर क्लिक झाल्यावर CrimeRegister फॉर्म ओपन करणे
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return RedirectToAction("Index", "CrimeRegister", new { id = id });
        }



        // ✅ PRINT ROUTER
        [HttpGet]
        public IActionResult Print(int crimeId, string type)
        {
            switch (type)
            {
                case "Index":
                    return RedirectToAction("PrintIndex", "Print", new { id = crimeId });

                case "Checklist":
                    return RedirectToAction("PrintChecklist", "Print", new { id = crimeId });

                case "FIR":
                    return RedirectToAction("PrintFIR", "Print", new { id = crimeId });

                case "SpotPanchnama":
                    return RedirectToAction("PrintSpotPanchnama", "Print", new { id = crimeId });

                case "Sample":
                    return RedirectToAction("PrintSample", "Print", new { id = crimeId });

                case "FinalReport":
                    return RedirectToAction("PrintFinalReport", "Print", new { id = crimeId });

                default:
                    return BadRequest("Invalid print type");
            }
        }

    }
}
