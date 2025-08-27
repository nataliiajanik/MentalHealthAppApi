using MentalHealthAppApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthAppApi.Controllers
{
    [ApiController]
    [Route("api/diseasePrediction")]
    public class DiseasePredictionController : ControllerBase
    {
        //sesja z załadowanym modelem ONNX aby można było go wielokrotnie wyłowywać bez osobnego załadowywania
        private readonly DiseasePredictionModelService _modelService;

        public DiseasePredictionController(DiseasePredictionModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpPost]
        public IActionResult PredictDisease([FromBody] int[] answers)
        {
            //IActionResult rezultat akcji kontrolera
            try
            {
                var predictionProbabilites = _modelService.PredictProbability(answers)
                    .Select(p => p * 100)
                    .ToArray();

                return Ok( new { predictionProbabilites });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
