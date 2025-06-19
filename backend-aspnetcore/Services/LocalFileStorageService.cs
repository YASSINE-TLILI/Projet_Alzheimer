using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using backend_aspnetcore.Settings;

namespace backend_aspnetcore.Services
{
	public class LocalFileStorageService : IFileStorageService
	{
		private readonly FileStorageSettings _settings;
		private readonly ILogger<LocalFileStorageService> _logger;

		public LocalFileStorageService(
			IOptions<FileStorageSettings> settings,
			ILogger<LocalFileStorageService> logger)
		{
			_settings = settings.Value;
			_logger = logger;

			// Créer le dossier s'il n'existe pas
			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), _settings.UploadPath);
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
				_logger.LogInformation($"Dossier de stockage créé: {uploadPath}");
			}
		}

		public async Task<string> SaveFileAsync(IFormFile file)
		{
			// Générer un nom de fichier unique
			var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
			var filePath = Path.Combine(_settings.UploadPath, uniqueFileName);

			_logger.LogDebug($"Sauvegarde du fichier: {filePath}");

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return filePath;
		}

		public void DeleteFile(string filePath)
		{
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
				_logger.LogDebug($"Fichier supprimé: {filePath}");
			}
			else
			{
				_logger.LogWarning($"Tentative de suppression d'un fichier inexistant: {filePath}");
			}
		}
	}
}