using MentalHealthAppApi.Models;
using MentalHealthAppApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthAppApi.Controllers
{
    [ApiController]
    [Route("api/diseasesPredictions")]
    public class DiseasePredictionController : ControllerBase
    {
        //sesja z załadowanym modelem ONNX aby można było go wielokrotnie wyłowywać bez osobnego załadowywania
        private readonly DiseasePredictionModelService _modelService;

        public DiseasePredictionController(DiseasePredictionModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpPost]
        public IActionResult PredictDisease([FromBody] DiseasePredictionRequest request)
        {
            //IActionResult rezultat akcji kontrolera
            try
            {
                var predictionProbabilites = _modelService.PredictProbability(request.Answers)
                    .Select(p => p * 100)
                    .ToArray();

                string[] diseaseNames = { "Bipolar disorder", "Schizophrenia", "Depression", "Anxiety disorder", "PTSD" };

                var result = diseaseNames
                    .Zip(predictionProbabilites, (name, prob) => new DiseasePredictionResult { Disease = name, Probability = prob })
                    .ToList();

                return Ok( new DiseasePredictionResponse { Predictions = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
