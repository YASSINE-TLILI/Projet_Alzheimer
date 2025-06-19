using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace frontend_razor.Models
{
    public class PredictionResult
    {
        [Required]
        public string PredictedClass { get; set; } = string.Empty; // Initialisation ajoutée

        [Required]
        public float Confidence { get; set; }

        [Required]
        public Dictionary<string, float> Probabilities { get; set; } = new Dictionary<string, float>(); // Initialisation ajoutée
    }
}