using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using frontend_razor.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using frontend_razor.Models;
using System.Text.Json;
namespace frontend_razor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDetectionService _detectionService;

        public IndexModel(IDetectionService detectionService)
        {
            _detectionService = detectionService;
        }

        [BindProperty]
        new public IFormFile? File { get; set; } // 'new' ajouté

        public PredictionResult? Result { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (File == null || File.Length == 0)
            {
                ModelState.AddModelError("File", "Veuillez sélectionner un fichier");
                return Page();
            }

            try
            {
                var response = await _detectionService.AnalyzeImage(File);
                TempData["DetectionResponse"] = JsonSerializer.Serialize(response);
                return RedirectToPage("Result");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erreur lors de l'analyse: {ex.Message}");
                return Page();
            }
        }
    }
}