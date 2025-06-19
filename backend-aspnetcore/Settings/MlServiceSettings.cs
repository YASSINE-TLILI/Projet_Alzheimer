using System.ComponentModel.DataAnnotations;

namespace backend_aspnetcore.Settings
{
    public class MlServiceSettings
    {
        [Required]
        public string BaseUrl { get; set; } = "http://localhost:8000/";
    }
}
