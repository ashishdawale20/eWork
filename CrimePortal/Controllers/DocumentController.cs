using CrimePortal.Models;
using Microsoft.AspNetCore.Mvc;
using Xceed.Words.NET;

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

            // Load template
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Template.docx");
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Generated", $"{model.CaseNumber}.docx");

            using (DocX doc = DocX.Load(templatePath))
            {
                // Replace placeholders
                doc.ReplaceText("{CASE_NUMBER}", model.CaseNumber);
                doc.ReplaceText("{CASE_DATE}", model.CaseDate);
                doc.ReplaceText("{ACCUSER_NAME}", model.AccuserName);
                doc.ReplaceText("{ACCUSED_NAME}", model.AccusedName);
                doc.ReplaceText("{ADDRESS}", model.Address);

                doc.SaveAs(outputPath);
            }

            // Return generated file
            byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{model.CaseNumber}.docx");
        }
    }
}
