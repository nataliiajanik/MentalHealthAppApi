using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace MentalHealthAppApi.Services
{
    public class DiseasePredictionModelService
    {
        //inicjalizacja sesji, potem ładowanie modelu ONNX do pamięci i wykonywanie na nim predykcji
        private readonly InferenceSession _inferenceSession;
        public DiseasePredictionModelService(string modelPath) 
        {
            //ładywanie modelu ONNX
            _inferenceSession = new InferenceSession(modelPath);
        }

        public float[] PredictProbability(float[] answers)
        {
            if(answers.Length != 51)
            {
                throw new ArgumentException("Prediction expects 51 answers.");
            }

            // wielowymiarowy tensor, wejście do modelu predykcyjnego ONNX
            // DenseTensor przechowuje dane w formacie, które ONNX potrafi odczytać
            var inputTensor = new DenseTensor<float>(answers, new int[] { 1, 51 });

            // tworzy NamedOnnxValue z inputTensor i nazwy jaką ma input w modelu ONNX
            //NamedOnnxValue połączona nazwa + wartość wejścia/wyjścia
            //musi być listą bo Run w InferenceSession przyjmuje listę IEnumerable<NamedOnnxValue>
            var inputsOnnx = new List<NamedOnnxValue> {
                NamedOnnxValue.CreateFromTensor("input", inputTensor)
            };

            //uruchomienie predykcji
            using var results = _inferenceSession.Run(inputsOnnx);

            // results zawiera jedno wyjście, którt składa się z 5ciu prawdopodobieństw dla 5 klas
            // konwersja tensora do płaskiej listy
            var predictedProbabilities = results[0].AsEnumerable<float>().ToArray();

            return predictedProbabilities;
        }
    }
}

