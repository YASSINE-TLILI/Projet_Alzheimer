namespace backend_aspnetcore.Settings
{
	public class FileStorageSettings
	{
		/// <summary>
		/// Chemin relatif pour le stockage des fichiers uploadés
		/// </summary>
		public string UploadPath { get; set; } = "wwwroot/uploads";
	}
}