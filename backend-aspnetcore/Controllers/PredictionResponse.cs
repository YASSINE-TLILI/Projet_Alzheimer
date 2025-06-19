using System.ComponentModel.DataAnnotations;
namespace backend_aspnetcore.Models
{
	public class PredictionResponse
	{
		[Required]
		public string FileName { get; set; } = "unknown";

		[Required]
		public string FileType { get; set; } = "application/octet-stream";

		public long FileSize { get; set; }

		[Required]
		public PredictionResult Prediction { get; set; } = new PredictionResult();

		public DateTime Timestamp { get; set; }
	}
}