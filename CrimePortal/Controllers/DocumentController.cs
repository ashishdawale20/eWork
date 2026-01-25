using CrimePortal.Models;
using Microsoft.AspNetCore.Mvc;
using Xceed.Words.NET;
using System.IO;

namespace CrimePortal.Controllers
{
    public class DocumentController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CaseInfo model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Template file path
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Template.docx");
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Generated");

            // अगर folder exist नहीं है तो बना दो
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string outputPath = Path.Combine(outputDir, $"{model.CaseNumber}.docx");

            using (DocX doc = DocX.Load(templatePath))
            {
                // ✅ Replace placeholders with string values
                doc.ReplaceText("{CASE_NUMBER}", model.CaseNumber ?? "");
                doc.ReplaceText("{CASE_DATE}", model.CaseDate.ToString("dd-MM-yyyy")); // <-- Fix यहाँ है
                doc.ReplaceText("{ACCUSER_NAME}", model.AccuserName ?? "");
                doc.ReplaceText("{ACCUSED_NAME}", model.AccusedName ?? "");
                doc.ReplaceText("{ADDRESS}", model.Address ?? "");

                doc.SaveAs(outputPath);
            }

            // File को response में return करो
            byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
            return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                $"{model.CaseNumber}.docx");
        }
    }
}
