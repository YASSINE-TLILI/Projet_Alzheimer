// Services/IDetectionService.cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using frontend_razor.Models; // Utilisez le modèle du dossier Models

namespace frontend_razor.Services
{
    public interface IDetectionService
    {
        Task<DetectionResponse?> AnalyzeImage(IFormFile file);
    }
}