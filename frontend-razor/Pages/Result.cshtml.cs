using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using frontend_razor.Models;
namespace frontend_razor.Pages
{
    public class ResultModel : PageModel
    {
        public DetectionResponse? DetectionResponse { get; set; }

        public void OnGet()
        {
            if (TempData["DetectionResponse"] is string json)
            {
                DetectionResponse = JsonSerializer.Deserialize<DetectionResponse>(json);
            }
        }
    }
}