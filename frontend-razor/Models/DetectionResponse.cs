using frontend_razor.Models;
using System.Collections.Generic;

namespace frontend_razor.Models{
    public class DetectionResponse
    {
        public required string FileName { get; set; } // Ajout de 'required'

        public required PredictionResult Prediction { get; set; } // Ajout de 'required'
    }
}
