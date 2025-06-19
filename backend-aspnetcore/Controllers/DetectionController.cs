using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using backend_aspnetcore.Services;
using backend_aspnetcore.Models;

namespace backend_aspnetcore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetectionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<DetectionController> _logger;

        public DetectionController(
            IPredictionService predictionService,
            IFileStorageService fileStorageService,
            ILogger<DetectionController> logger)
        {
            _predictionService = predictionService;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        /// <summary>
        /// Analyse une image IRM pour détecter la maladie d'Alzheimer
        /// </summary>
        /// <param name="file">Fichier image (JPG, PNG) ou DICOM</param>
        /// <returns>Résultats de la prédiction</returns>
        [HttpPost("analyze")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PredictionResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Analyze(IFormFile file)
        {
            try
            {
                // 1. Validation du fichier
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("Tentative d'upload sans fichier");
                    return BadRequest("Aucun fichier fourni");
                }

                // 2. Vérification du type de fichier
                var validContentTypes = new[] {
                    "image/jpeg",
                    "image/png",
                    "application/dicom"
                };

                if (!validContentTypes.Contains(file.ContentType))
                {
                    _logger.LogWarning($"Type de fichier non supporté: {file.ContentType}");
                    return BadRequest($"Type de fichier non supporté. Types acceptés: {string.Join(", ", validContentTypes)}");
                }

                // 3. Vérification de la taille du fichier
                const int maxFileSize = 50 * 1024 * 1024; // 50MB
                if (file.Length > maxFileSize)
                {
                    _logger.LogWarning($"Fichier trop volumineux: {file.Length} bytes");
                    return BadRequest($"Fichier trop volumineux. Taille maximale: {maxFileSize / (1024 * 1024)}MB");
                }

                // 4. Journalisation
                _logger.LogInformation($"Analyse demandée pour: {file.FileName} ({file.ContentType}, {file.Length} bytes)");

                // 5. Sauvegarde temporaire du fichier
                var filePath = await _fileStorageService.SaveFileAsync(file);
                _logger.LogDebug($"Fichier sauvegardé temporairement: {filePath}");

                // 6. Appel au service de prédiction
                var predictionResult = await _predictionService.PredictAsync(file);
                _logger.LogInformation($"Prédiction réussie: {predictionResult.Class}");

                // 7. Construction de la réponse
                var response = new PredictionResponse
                {
                    FileName = file.FileName,
                    FileType = file.ContentType,
                    FileSize = file.Length,
                    Prediction = predictionResult,
                    Timestamp = DateTime.UtcNow
                };

                // 8. Nettoyage (optionnel)
                _fileStorageService.DeleteFile(filePath);
              

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de l'analyse du fichier {file?.FileName}");
                
                return StatusCode(500, "Une erreur interne est survenue lors du traitement");
            }
        }

        /// <summary>
        /// Vérification de la santé du service
        /// </summary>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "OK",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}