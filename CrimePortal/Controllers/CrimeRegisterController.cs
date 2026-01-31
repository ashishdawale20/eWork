using System;
using CrimePortal.Data;
using CrimePortal.Models;
using CrimePortal.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using DocumentFormat.OpenXml;

namespace CrimePortal.Controllers
{
    public class CrimeRegisterController : Controller
    {
        private readonly CRDbContext _crimecontext;

        private readonly AppDbContext _appcontext;

        public CrimeRegisterController(CRDbContext crimecontext, AppDbContext appcontext)
        {
            _crimecontext = crimecontext;

            _appcontext = appcontext;
        }


            

    // ✅ GET
    [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {

            int loginUserId = int.Parse(
    User.FindFirstValue(ClaimTypes.NameIdentifier)
);

            // 🔽 Dropdown Data (ONLY login user)
            var complainants = await _appcontext.Jawans
                .Where(x => x.UserId == loginUserId)
                .Select(x => new SelectListItem
                {
                    Value = x.JawanName,
                    Text = x.JawanName
                })
                .ToListAsync();

            var carriers = await _appcontext.Jawans
                .Where(x => x.UserId == loginUserId)
                .Select(x => new SelectListItem
                {
                    Value = x.JawanName,
                    Text = x.JawanName
                }).ToListAsync();

            var officerNames = await _appcontext.Officers
                .Where(x => x.UserId == loginUserId)
                .Select(x => new SelectListItem
                {
                    Value = x.OfficerName,
                    Text = x.OfficerName
                })
                .Distinct()
                .ToListAsync();

            var officerRanks = await _appcontext.Officers
                .Where(x => x.UserId == loginUserId)
                .Select(x => new SelectListItem
                {
                    Value = x.OfficerRank,
                    Text = x.OfficerRank
                })
                .Distinct()
                .ToListAsync();


            var office = await _appcontext.Offices
    .FirstOrDefaultAsync(x => x.UserId == loginUserId);

            if (id == null)
            {
                return View(new CrimeRegisterViewModel
                {
                    UserId = loginUserId,
                    OfficerRank = office?.Post,
                    OfficeName = office?.OfficeName,
                    OfficeAddress = office?.OfficeAddress,
                    District = office?.District,
                    Division = office?.Division,

                    ComplainantList = complainants,
                    CarrierList = carriers,
                    OfficerNameList = officerNames,
                    OfficerRankList = officerRanks,

                    Year = DateTime.Now.Year,
                    CrimeDate = DateTime.Today,
                    Panchas = new List<Pancha> { new Pancha(), new Pancha() },
                    Samples = new List<Sample> { new Sample() },
                    SeizedItems = new List<SeizedItem> { new SeizedItem() }
                });
            }

            var crime = await _crimecontext.CrimeRegisters
                .Include(c => c.Panchas)
                .Include(c => c.SeizedItems)
                .Include(c => c.Samples)
                .FirstOrDefaultAsync(c =>
        c.CrimeRegisterId == id.Value &&
        c.UserId == loginUserId
    );

            if (crime == null)
                return NotFound();

            return View(new CrimeRegisterViewModel
            {
                CrimeRegisterId = crime.CrimeRegisterId,
                Year = crime.Year,
                CaseNumber = crime.CaseNumber,
                CrimeDate = crime.CrimeDate,
                TimeFrom = crime.TimeFrom,
                TimeTo = crime.TimeTo,
                Complainant = crime.Complainant,
                AccusedName = crime.AccusedName,
                AccusedAge = crime.AccusedAge,
                AccusedGender = crime.AccusedGender,
                AccusedRelativeName = crime.AccusedRelativeName,
                AccusedAddress = crime.AccusedAddress,
                PlaceOfIncident = crime.PlaceOfIncident,
                Act = crime.Act,
                Court = crime.Court,
                PoliceStation = crime.PoliceStation,
                InvestigatingOfficerName = crime.InvestigatingOfficerName,
                InvestigatingOfficerRank = crime.InvestigatingOfficerRank,

                UserId = loginUserId,
                OfficerRank = office?.Post,
                OfficeName = office?.OfficeName,
                OfficeAddress = office?.OfficeAddress,
                District = office?.District,
                Division = office?.Division,

                ComplainantList = complainants,
                CarrierList = carriers,
                OfficerNameList = officerNames,
                OfficerRankList = officerRanks,

                HandBhattiLiters = crime.HandBhattiLiters,
                MohasavaLiters = crime.MohasavaLiters,
                DeshiLiquorLiters = crime.DeshiLiquorLiters,
                ForeignLiquorLiters = crime.ForeignLiquorLiters,
                BeerLiters = crime.BeerLiters,

                CarrierName = crime.CarrierName,
                SampleCount = crime.SampleNumber,
                TotalSeizedValue = crime.TotalSeizedValue,

                Panchas = crime.Panchas.ToList(),
                SeizedItems = crime.SeizedItems.ToList(),
                Samples = crime.Samples.ToList()
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CrimeRegisterViewModel viewModel)
        {
            string loginUserName = User.Identity.Name;

            var user = _appcontext.Users
                .FirstOrDefault(u => u.UserName == loginUserName);

            if (user == null)
                return Unauthorized();

            int loginUserId = user.UserId;

            if (!ModelState.IsValid)

            {
                // 🔴 ModelState errors collect करा
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                TempData["SaveError"] =
                    string.Join(" | ", errors.SelectMany(e => e.Errors));



                bool exists = await _crimecontext.CrimeRegisters.AnyAsync(c =>
    c.Year == viewModel.Year &&
    c.CaseNumber == viewModel.CaseNumber &&
    c.CrimeRegisterId != viewModel.CrimeRegisterId &&
    c.UserId == loginUserId
);

                if (exists)
                {
                    ModelState.AddModelError("CaseNumber",
                        "या वर्षात हा गुन्हा क्रमांक आधीच नोंदवलेला आहे.");

                    return View(viewModel);
                }


                viewModel.ComplainantList = await _appcontext.Jawans
                    .Where(x => x.UserId == loginUserId)
                    .Select(x => new SelectListItem
                    {
                        Value = x.JawanName,
                        Text = x.JawanName
                    }).ToListAsync();

                viewModel.CarrierList = await _appcontext.Jawans
                    .Where(x => x.UserId == loginUserId)
                    .Select(x => new SelectListItem
                    {
                        Value = x.JawanName,
                        Text = x.JawanName
                    }).ToListAsync();

                viewModel.OfficerNameList = await _appcontext.Officers
                    .Where(x => x.UserId == loginUserId)
                    .Select(x => new SelectListItem
                    {
                        Value = x.OfficerName,
                        Text = x.OfficerName
                    }).Distinct().ToListAsync();

                viewModel.OfficerRankList = await _appcontext.Officers
                    .Where(x => x.UserId == loginUserId)
                    .Select(x => new SelectListItem
                    {
                        Value = x.OfficerRank,
                        Text = x.OfficerRank
                    }).Distinct().ToListAsync();

                return View(viewModel);
            }


            try
            {
                // ✅ Step 1: तपासा - CrimeRegister आधीपासून आहे का?
                var existing = await _crimecontext.CrimeRegisters
                    .Include(c => c.Panchas)
                    .Include(c => c.SeizedItems)
                    .Include(c => c.Samples)
                    .FirstOrDefaultAsync(c => c.CrimeRegisterId == viewModel.CrimeRegisterId);

                if (existing == null)
                {

                    MapGenderWords(
    viewModel.AccusedGender,
    out string personWord,
    out string personToWord,
    out string properWord
);


                    // 🆕 नवीन गुन्हा तयार करा
                    var crime = new CrimeRegister
                    {
                        Year = viewModel.Year,
                        CaseNumber = viewModel.CaseNumber,
                        CrimeDate = viewModel.CrimeDate.Kind == DateTimeKind.Unspecified 
                            ? DateTime.SpecifyKind(viewModel.CrimeDate, DateTimeKind.Utc)
                            : viewModel.CrimeDate.ToUniversalTime(),
                        TimeFrom = viewModel.TimeFrom,
                        TimeTo = viewModel.TimeTo,
                        Complainant = viewModel.Complainant,
                        AccusedName = viewModel.AccusedName,
                        AccusedAge = viewModel.AccusedAge,
                        AccusedGender = viewModel.AccusedGender,
                        PersonWord = personWord,
                        PersonToWord = personToWord,
                        ProperWord = properWord,
                        AccusedRelativeName = viewModel.AccusedRelativeName,
                        AccusedAddress = viewModel.AccusedAddress,
                        PlaceOfIncident = viewModel.PlaceOfIncident,
                        Act = viewModel.Act,
                        Court = viewModel.Court,
                        PoliceStation = viewModel.PoliceStation,
                        InvestigatingOfficerName = viewModel.InvestigatingOfficerName,
                        InvestigatingOfficerRank = viewModel.InvestigatingOfficerRank,
                        UserId = loginUserId,
                        OfficerRank = viewModel.OfficerRank,
                        OfficeName = viewModel.OfficeName,
                        OfficeAddress = viewModel.OfficeAddress,
                        District = viewModel.District,
                        Division = viewModel.Division,


                        HandBhattiLiters = viewModel.HandBhattiLiters,
                        MohasavaLiters = viewModel.MohasavaLiters,
                        DeshiLiquorLiters = viewModel.DeshiLiquorLiters,
                        ForeignLiquorLiters = viewModel.ForeignLiquorLiters,
                        BeerLiters = viewModel.BeerLiters,

                        CarrierName = viewModel.CarrierName,
                        SampleNumber = viewModel.SampleCount ?? 0,
                        TotalSeizedValue = viewModel.TotalSeizedValue ?? 0m,
                        Panchas = viewModel.Panchas?.Where(p => !string.IsNullOrWhiteSpace(p.Name)).ToList() ?? new(),
                        SeizedItems = viewModel.SeizedItems?.Where(s => !string.IsNullOrWhiteSpace(s.PropertyType)).ToList() ?? new(),
                        Samples = viewModel.Samples?.Where(s => !string.IsNullOrWhiteSpace(s.SampleType)).ToList() ?? new()


                    };

                    _crimecontext.CrimeRegisters.Add(crime);
                }
                else
                {
                    // ✏️ Existing record अपडेट करा
                    existing.Year = viewModel.Year;
                    existing.CaseNumber = viewModel.CaseNumber;
                    existing.CrimeDate = viewModel.CrimeDate.Kind == DateTimeKind.Unspecified 
                        ? DateTime.SpecifyKind(viewModel.CrimeDate, DateTimeKind.Utc)
                        : viewModel.CrimeDate.ToUniversalTime();
                    existing.TimeFrom = viewModel.TimeFrom;
                    existing.TimeTo = viewModel.TimeTo;
                    existing.Complainant = viewModel.Complainant;
                    existing.AccusedName = viewModel.AccusedName;
                    existing.AccusedAge = viewModel.AccusedAge;
                    MapGenderWords(
    viewModel.AccusedGender,
    out string personWord,
    out string personToWord,
    out string properWord
);

                    existing.AccusedGender = viewModel.AccusedGender;
                    existing.PersonWord = personWord;
                    existing.PersonToWord = personToWord;
                    existing.ProperWord = properWord;
                    existing.AccusedRelativeName = viewModel.AccusedRelativeName;
                    existing.AccusedAddress = viewModel.AccusedAddress;
                    existing.PlaceOfIncident = viewModel.PlaceOfIncident;
                    existing.Act = viewModel.Act;
                    existing.Court = viewModel.Court;
                    existing.PoliceStation = viewModel.PoliceStation;
                    existing.InvestigatingOfficerName = viewModel.InvestigatingOfficerName;
                    existing.InvestigatingOfficerRank = viewModel.InvestigatingOfficerRank;
                    existing.UserId = loginUserId;
                    existing.OfficerRank = viewModel.OfficerRank;
                    existing.OfficeName = viewModel.OfficeName;
                    existing.OfficeAddress = viewModel.OfficeAddress;
                    existing.District = viewModel.District;
                    existing.Division = viewModel.Division;
                    existing.CarrierName = viewModel.CarrierName;
                    existing.SampleNumber = viewModel.SampleCount ?? 0;
                    existing.TotalSeizedValue = viewModel.TotalSeizedValue ?? 0m;

                    existing.HandBhattiLiters = viewModel.HandBhattiLiters;
                    existing.MohasavaLiters = viewModel.MohasavaLiters;
                    existing.DeshiLiquorLiters = viewModel.DeshiLiquorLiters;
                    existing.ForeignLiquorLiters = viewModel.ForeignLiquorLiters;
                    existing.BeerLiters = viewModel.BeerLiters;

                    // 🔄 जुने Panchas, Items, Samples delete करा
                    _crimecontext.Panchas.RemoveRange(existing.Panchas);
                    _crimecontext.SeizedItems.RemoveRange(existing.SeizedItems);
                    _crimecontext.Samples.RemoveRange(existing.Samples);
                    await _crimecontext.SaveChangesAsync();

                    // नवीन values सेट करा
                    existing.Panchas = viewModel.Panchas?.Where(p => !string.IsNullOrWhiteSpace(p.Name)).ToList() ?? new();
                    existing.SeizedItems = viewModel.SeizedItems?.Where(s => !string.IsNullOrWhiteSpace(s.PropertyType)).ToList() ?? new();
                    existing.Samples = viewModel.Samples?.Where(s => !string.IsNullOrWhiteSpace(s.SampleType)).ToList() ?? new();

                    _crimecontext.CrimeRegisters.Update(existing);
                }

                await _crimecontext.SaveChangesAsync();
                TempData["Success"] = "गुन्ह्याची माहिती यशस्वीरित्या सेव झाली!";
                return RedirectToAction("Index", new { id = (int?)null });
            }
            catch (Exception ex)
            {
                TempData["SaveError"] = "Error: " + ex.Message;
                return View(viewModel);
            }
        }


        [HttpGet]
        public IActionResult CheckCaseNumber(int year, int caseNumber, int id = 0)
        {
            int loginUserId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            bool exists = _crimecontext.CrimeRegisters.Any(c =>
                c.Year == year &&
                c.CaseNumber == caseNumber.ToString() &&
                c.CrimeRegisterId != id &&
                c.UserId == loginUserId          // ⭐ IMPORTANT
            );

            return Json(!exists);
        }



        [HttpGet]
        public async Task<IActionResult> PrintReport(int id, string type)
        {
            int loginUserId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            var crime = await _crimecontext.CrimeRegisters
                .Include(c => c.Panchas)
                .Include(c => c.SeizedItems)
                .Include(c => c.Samples)
                .FirstOrDefaultAsync(c =>
                    c.CrimeRegisterId == id &&
                    c.UserId == loginUserId);

            if (crime == null)
                return NotFound();

            string templateFile = type switch
            {
                "Index" => "IndexTemplate.docx",
                "Checklist" => "ChecklistTemplate.docx",
                "FIR" => "FIRTemplate.docx",
                "SpotPanchnama" => "SpotPanchnamaTemplate.docx",
                "Sample" => "SampleTemplate.docx",
                "FinalReport" => "FinalReportTemplate.docx",
                _ => "CrimeReportTemplate.docx"
            };

            string templatePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Templates",
                templateFile
            );

            string outputPath = Path.Combine(
                Path.GetTempPath(),
                $"{type}_{crime.CaseNumber}.docx"
            );

            System.IO.File.Copy(templatePath, outputPath, true);

            using (WordprocessingDocument doc =
                WordprocessingDocument.Open(outputPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                // 🔹 SIMPLE MERGE
                ReplaceText(body, "{Accuser_name}", crime.AccusedName);
                ReplaceText(body, "{Accused_Age}", crime.AccusedAge?.ToString());
                ReplaceText(body, "{Accused_Address}", crime.AccusedAddress);
                ReplaceText(body, "{Complainant}", crime.Complainant);
                ReplaceText(body, "{InvestigatingOfficerName}", crime.InvestigatingOfficerName);
                ReplaceText(body, "{InvestigatingOfficerRank}", crime.InvestigatingOfficerRank);
                ReplaceText(body, "{OfficerRank}", crime.OfficerRank);
                ReplaceText(body, "{OfficeName}", crime.OfficeName);
                ReplaceText(body, "{Office_Address}", crime.OfficeAddress);
                ReplaceText(body, "{PlaceOfincident}", crime.PlaceOfIncident);
                ReplaceText(body, "{CaseNumber}", crime.CaseNumber);
                ReplaceText(body, "{District}", crime.District);
                ReplaceText(body, "{Division}", crime.Division);
                ReplaceText(body, "{PoliceStation}", crime.PoliceStation);
                ReplaceText(body, "{Sex}", crime.AccusedGender);
                ReplaceText(body, "{PersonWord}", crime.PersonWord);
                ReplaceText(body, "{PersonToWord}", crime.PersonToWord);
                ReplaceText(body, "{ProperWord}", crime.ProperWord);
                ReplaceText(body, "{AccusedRelativeName}", crime.AccusedRelativeName);
                ReplaceText(body, "{CarrierName}", crime.CarrierName);
                ReplaceText(body, "{Year}", crime.Year.ToString());
                ReplaceText(
    body,
    "{CrimeDate}",
    crime.CrimeDate.ToString("dd/MM/yyyy")
);
                ReplaceText(
    body,
    "{TimeFrom}",
    crime.TimeFrom.HasValue
        ? crime.TimeFrom.Value.ToString(@"hh\:mm")
        : ""
);
                ReplaceText(
    body,
    "{TimeTo}",
    crime.TimeTo.HasValue
        ? crime.TimeTo.Value.ToString(@"hh\:mm")
        : ""
);
                ReplaceText(body, "{Court}", crime.Court);
                ReplaceText(body, "{Act}", crime.Act);
                ReplaceText(body, "{SampleNumber}", crime.SampleNumber.ToString());
                
                
                decimal total = 0;

                // अगर SeizedItems हैं तो calculate करो
                if (crime.SeizedItems != null && crime.SeizedItems.Any())
                {
                    total = crime.SeizedItems.Sum(x => x.ApproxValue ?? 0);
                }
                else
                {
                    total = crime.TotalSeizedValue ?? 0m; // fallback अगर कोई item नहीं है
                }

                ReplaceText(
                    body,
                    "{TotalSeizedValue}",
                    FormatMarathiAmount(total)
                );



                // 🔹 PANCHAS
                for (int i = 0; i < crime.Panchas.Count; i++)
                {
                    ReplaceText(body, $"{{Pancha{i + 1}Name}}", crime.Panchas[i].Name);
                    ReplaceText(body, $"{{Pancha{i + 1}Age}}", crime.Panchas[i].Age?.ToString());
                    ReplaceText(body, $"{{Pancha{i + 1}Address}}", crime.Panchas[i].Address);
                }



                // 🔹 SAMPLE TEXT (INLINE तयार करा)
                string sampleText = string.Join(", ",
                    crime.Samples.Select(s => s.SampleType)
                );


                ReplaceInlineText(
    body,
    "{Samples}",
    sampleText
);




                if (type == "FIR")
                {
                    FillSeizedItemsInFIRTable(body, crime.SeizedItems);
                }



                else if (type == "FinalReport" || type == "SpotPanchnama")
                {
                    FillSeizedItemsInExistingTable(
                        body,
                        crime.SeizedItems
                    );
                }





                doc.MainDocumentPart.Document.Save();
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                $"{type}_{crime.CaseNumber}.docx"
            );
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



        #region Word Helper Methods

        private void ReplaceText(Body body, string placeholder, string value)
        {
            foreach (var text in body.Descendants<Text>())
            {
                if (text.Text.Contains(placeholder))
                {
                    text.Text = text.Text.Replace(placeholder, value ?? "");
                }
            }
        }


        // ✅ FIR ITEM ROW marker hide (fragment-safe)
        private void HideFirItemRowMarker(TableRow row)
        {
            foreach (var run in row.Descendants<Run>())
            {
                string combinedText = string.Concat(
                    run.Elements<Text>().Select(t => t.Text)
                );

                if (combinedText.Contains("FIR_ITEM_ROW"))
                {
                    foreach (var text in run.Elements<Text>())
                    {
                        text.Text = "";
                    }
                }
            }
        }



        private void FillSeizedItemsInExistingTable(
      Body body,
      List<SeizedItem> items
  )
        {
            var table = body.Descendants<Table>()
                .FirstOrDefault(t => t.Descendants<TableRow>().Count() >= 2);

            if (table == null)
                return;

            var totalRow = table.Elements<TableRow>().Last();

            int sr = 1;
            decimal total = 0;

            foreach (var item in items)
            {
                decimal value = item.ApproxValue ?? 0;

                var row = new TableRow(
                    CreateCell(sr.ToString()),
                    CreateCell(item.Description),
                    CreateCell(FormatMarathiAmount(value))  // 👈 FORMAT HERE
                );

                table.InsertBefore(row, totalRow);

                total += value;
                sr++;
            }

            // total cell
            var totalCell = totalRow.Elements<TableCell>().Last();
            totalCell.RemoveAllChildren<Paragraph>();
            totalCell.Append(
                new Paragraph(new Run(new Text(FormatMarathiAmount(total))))
            );
        }





        private void FillSeizedItemsInFIRTable(
     Body body,
     List<SeizedItem> items
 )
        {
            if (items == null || !items.Any())
                return;

            // 1️⃣ FIR table लो
            var table = body.Descendants<Table>()
                .FirstOrDefault(t => t.Elements<TableRow>().Count() > 2);

            if (table == null)
                return;

            // 2️⃣ Total row (last)
            var totalRow = table.Elements<TableRow>().Last();

            // 3️⃣ TEMPLATE ROW
            // 👉 FIR template में जो खाली data-row है
            var templateRow = table.Descendants<TableRow>()
    .FirstOrDefault(r =>
        r.InnerText.Contains("{{FIR_ITEM_ROW}}")
    );

            if (templateRow == null)
                return;


            // ✅ marker hide ONE TIME
            HideFirItemRowMarker(templateRow);

            int sr = 1;
            decimal total = 0;

            foreach (var item in items)
            {
                decimal value = item.ApproxValue ?? 0;

                // 🔹 1. Template row CLONE
                TableRow newRow = (TableRow)templateRow.CloneNode(true);

                
                

                // 🔹 3. Cells list
                var cells = newRow.Elements<TableCell>().ToList();

                // 🔹 4. Cell mapping (EXACT order)
                SetCell(cells[0], "");                              // Empty
                SetCell(cells[1], sr.ToString());                   // Sr no
                SetCell(cells[2], item.PropertyClass);              // Property category
                SetCell(cells[3], item.PropertyType);               // Property type
                SetCell(cells[4], item.Description);                // Description
                SetCell(cells[5], FormatMarathiAmount(value));      // Value

                // 🔹 5. Insert before total row
                table.InsertBefore(newRow, totalRow);

                total += value;
                sr++;
            }


            // 7️⃣ Update total
            var totalCell = totalRow.Elements<TableCell>().Last();
            SetCell(totalCell, FormatMarathiAmount(total));
        }





        private void RemoveVerticalMerge(TableCell cell)
        {
            var props = cell.GetFirstChild<TableCellProperties>();
            if (props == null) return;

            var vMerge = props.Elements<VerticalMerge>().FirstOrDefault();
            if (vMerge != null)
                vMerge.Remove();
        }















        private Run CreateRun(string text)
        {
            var run = new Run();

            run.RunProperties = new RunProperties(
                new RunFonts
                {
                    Ascii = "DVOT-SurekhMR",
                    HighAnsi = "DVOT-SurekhMR"
                },
                new FontSize { Val = "24" } // 12pt
            );

            run.Append(
                new Text(text ?? "")
                {
                    Space = SpaceProcessingModeValues.Preserve
                }
            );

            return run;
        }


        private TableCell CreateCell(string text)
        {
            var run = new Run();

            run.RunProperties = new RunProperties(
                new RunFonts
                {
                    Ascii = "DVOT-SurekhMR",
                    HighAnsi = "DVOT-SurekhMR"
                },
                new FontSize { Val = "24" } // 12pt
            );

            run.Append(
                new Text(text ?? "")
                {
                    Space = SpaceProcessingModeValues.Preserve
                }
            );

            return new TableCell(
                new Paragraph(run)
            );
        }



        private void SetCell(TableCell cell, string text)
        {
            cell.RemoveAllChildren<Paragraph>();

            var paragraph = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts
                        {
                            Ascii = "DVOT-SurekhMR",
                            HighAnsi = "DVOT-SurekhMR"
                        },
                        new FontSize { Val = "24" }
                    ),
                    new Text(text ?? "")
                    {
                        Space = SpaceProcessingModeValues.Preserve
                    }
                )
            );

            cell.Append(paragraph);
        }



        // ✅ Amount cell – DVOT-Surekh font only
        private void SetAmountCell(TableCell cell, string text)
        {
            cell.RemoveAllChildren<Paragraph>();

            var paragraph = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                ),
                new Run(
                    new RunProperties(
                        new RunFonts
                        {
                            Ascii = "DVOT-Surekh",
                            HighAnsi = "DVOT-Surekh"
                        },
                        new FontSize { Val = "24" }
                    ),
                    new Text(text ?? "")
                    {
                        Space = SpaceProcessingModeValues.Preserve
                    }
                )
            );

            cell.Append(paragraph);
        }




        private TableRow CreateHeaderRow(params string[] headers)
        {
            var row = new TableRow();

            foreach (var header in headers)
            {
                row.Append(
                    new TableCell(
                        new Paragraph(
                            new Run(new Text(header))
                        )
                    )
                );
            }

            return row;
        }

        private TableRow CreateRow(params string[] values)
        {
            var row = new TableRow();

            foreach (var value in values)
            {
                row.Append(
                    new TableCell(
                        new Paragraph(
                            new Run(new Text(value))
                        )
                    )
                );
            }

            return row;
        }


        private void ReplaceInlineText(Body body, string placeholder, string value)
        {
            foreach (var paragraph in body.Descendants<Paragraph>())
            {
                var runs = paragraph.Elements<Run>().ToList();
                if (!runs.Any()) continue;

                string fullText = string.Concat(
                    runs.Select(r => r.GetFirstChild<Text>()?.Text)
                );

                if (!fullText.Contains(placeholder))
                    continue;

                // placeholder असलेला paragraph मिळाला
                foreach (var run in runs)
                {
                    var text = run.GetFirstChild<Text>();
                    if (text == null) continue;

                    if (text.Text.Contains(placeholder))
                    {
                        text.Text = text.Text.Replace(
                            placeholder,
                            value ?? ""
                        );
                    }
                }
            }
        }


        private string FormatMarathiAmount(decimal? amount)
        {
            if (!amount.HasValue)
                return ConvertToMarathiDigits("0") + "/-";

            var integerPart = Math.Floor(amount.Value).ToString("0");
            return ConvertToMarathiDigits(integerPart) + "/-";
        }

        private string ConvertToMarathiDigits(string input)
        {
            char[] marathiDigits = { '०', '१', '२', '३', '४', '५', '६', '७', '८', '९' };
            return string.Concat(input.Select(c => char.IsDigit(c) ? marathiDigits[c - '0'] : c));
        }



        #endregion


        private void MapGenderWords(
    string gender,
    out string personWord,
    out string personToWord,
    out string properWord
)
        {
            personWord = "";
            personToWord = "";
            properWord = "";

            if (gender == "पुरुष")
            {
                personWord = "इसम";
                personToWord = "इसमास";
                properWord = "याचे";
            }
            else if (gender == "स्त्री")
            {
                personWord = "महिला";
                personToWord = "महिलेस";
                properWord = "हिचे";
            }
        }


    }
}
