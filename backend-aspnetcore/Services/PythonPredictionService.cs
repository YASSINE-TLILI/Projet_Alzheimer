using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using backend_aspnetcore.Models;

// <--- corriger ce namespace selon votre structure réelle
using backend_aspnetcore.Settings; // <--- corriger ce namespace selon votre structure réelle
using System.ComponentModel.DataAnnotations;
namespace backend_aspnetcore.Services
{
    public class PythonPredictionService : IPredictionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PythonPredictionService> _logger;
        private readonly MlServiceSettings _settings;

        public PythonPredictionService(
            HttpClient httpClient,
            ILogger<PythonPredictionService> logger,
            IOptions<MlServiceSettings> settings) // Correction du type
        {
            _httpClient = httpClient;
            _logger = logger;
            _settings = settings.Value; // Accès à la valeur des paramètres

            // Configuration de base de l'HttpClient
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<PredictionResult> PredictAsync(IFormFile file)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                // Préparation du contenu du fichier
                var fileContent = new StreamContent(file.OpenReadStream())
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue(file.ContentType),
                        ContentLength = file.Length
                    }
                };

                content.Add(fileContent, "file", file.FileName);
                
                _logger.LogDebug($"Envoi au service ML: {_settings.BaseUrl}/predict");

                // Envoi de la requête
                var response = await _httpClient.PostAsync("predict", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erreur du service ML: {response.StatusCode} - {errorContent}");
                    throw new Exception($"Erreur du service ML: {response.StatusCode}");
                }

                // Lecture et désérialisation de la réponse
                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogDebug($"Réponse du service ML: {jsonResponse}");

                var apiResponse = JsonConvert.DeserializeObject<MlApiResponse>(jsonResponse);

                if (apiResponse?.Status != "success")
                {
                    _logger.LogError($"Réponse invalide du service ML: {jsonResponse}");
                    throw new Exception("Réponse invalide du service ML");
                }

                return apiResponse.Prediction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la prédiction");
                throw;
            }
        }
    }

    // Classes pour la désérialisation de la réponse JSON
    public class MlApiResponse
    {
        [Required]
        public string Status { get; set; } = "unknown";

        [Required]
        public string Filename { get; set; } = "unknown";

        [Required]
        public string ContentType { get; set; } = "application/octet-stream";

        [Required]
        public PredictionResult Prediction { get; set; } = new PredictionResult();
    }
}