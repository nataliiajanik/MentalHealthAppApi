namespace MentalHealthAppApi.Models
{
    public class DiseasePredictionResult
    {
        public required string Disease {  get; set; }
        public float Probability {  get; set; }
    }

    public class DiseasePredictionResponse
    {
        public List<DiseasePredictionResult> Predictions { get; set; } = [];
    }
}
