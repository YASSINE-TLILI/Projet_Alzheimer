using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using backend_aspnetcore.Models;

namespace backend_aspnetcore.Services
{
    public interface IPredictionService
    {
        Task<PredictionResult> PredictAsync(IFormFile file);
    }
}