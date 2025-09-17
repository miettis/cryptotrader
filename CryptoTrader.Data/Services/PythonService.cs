using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Json;

namespace CryptoTrader.Data.Services
{
    public class PythonService
    {
        private readonly string _baseUrl;
        public PythonService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public async Task Predict()
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}");
            var content = await response.Content.ReadAsStringAsync();
            ;
        }

        public async Task<SavgolResponseValues[]> GetSavgol(SavgolRequest request)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"{_baseUrl}/savgol", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SavgolResponseValues[]>();
            }
            return Array.Empty<SavgolResponseValues>();
        }
        public async Task<SavgolResponseValues[]> GetSavgolV2(SavgolRequest request)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"{_baseUrl}/savgol/v2", request);
            if (response.IsSuccessStatusCode)
            {
                //var json = await response.Content.ReadAsStringAsync();
                return await response.Content.ReadFromJsonAsync<SavgolResponseValues[]>();
            }
            return Array.Empty<SavgolResponseValues>();
        }
        public async Task<CandleStickResponse[]> GetCandleSticks(Price[] prices)
        {
            var request = new CandleStickRequest
            {
                Open = prices.Select(p => p.Open).ToArray(),
                High = prices.Select(p => p.High).ToArray(),
                Low = prices.Select(p => p.Low).ToArray(),
                Close = prices.Select(p => p.Close).ToArray()
            };
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"{_baseUrl}/candlestick", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CandleStickResponse[]>();
            }
            return Array.Empty<CandleStickResponse>();
        }

        public async Task<TrainResponse[]> TrainClassifiers(int cryptoId, DateTimeOffset start, DateTimeOffset end)
        {
            var request = new TrainRequest
            {
                CryptoId = cryptoId,
                StartTime = start,
                EndTime = end
            };
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromHours(1);
            var response = await client.PostAsJsonAsync($"{_baseUrl}/train-classifier", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TrainResponse[]>();
            }

            return Array.Empty<TrainResponse>();
        }
        public async Task<TrainResponse[]> TrainRegressors(int cryptoId, DateTimeOffset start, DateTimeOffset end)
        {
            var request = new TrainRequest
            {
                CryptoId = cryptoId,
                StartTime = start,
                EndTime = end
            };
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromHours(1);
            var response = await client.PostAsJsonAsync($"{_baseUrl}/train-regression", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TrainResponse[]>();
            }

            return Array.Empty<TrainResponse>();
        }
        public async Task<PredictionClassifierResponse> Predict(int cryptoId, DateTimeOffset time, IEnumerable<CryptoModel> models)
        {
            var request = new PredictionRequest
            {
                CryptoId = cryptoId,
                Time = time,
                Models = models.Select(x => new PredictionRequestModel
                {
                    Output = x.Output,
                    ModelName = x.ModelName
                }).ToArray()
            };
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromHours(1);
            var response = await client.PostAsJsonAsync($"{_baseUrl}/predict", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PredictionClassifierResponse>();
            }

            return null;
        }
    }


    public class SavgolRequest
    {
        public SavgolRequestValues[] Data { get; set; }
        public int[] Windows { get; set; }
        public int Order { get; set; }
    }
    public class SavgolRequestValues
    {
        public string Field { get; set; }
        public decimal[] Values { get; set; }
    }
    public class SavgolResponseValues
    {
        public string Field { get; set; }
        public int Window { get; set; }
        public int Order { get; set; }
        public decimal?[] Smooth { get; set; }
        public decimal?[] Derivatives { get; set; }
    }
    public class CandleStickRequest
    {
        public decimal[] Open { get; set; }
        public decimal[] High { get; set; }
        public decimal[] Low { get; set; }
        public decimal[] Close { get; set; }
    }

    public class CandleStickResponse
    {
        public string Pattern { get; set; }
        public int[] Result { get; set; }
    }

    public class TrainRequest
    {
        public int CryptoId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }

    public class TrainResponse
    {
        public string Output { get; set; }
        public string ModelName { get; set; }
        public decimal Accuracy { get; set; }
        public int Samples { get; set; }
    }

    public class PredictionRequest
    {
        public int CryptoId { get; set; }
        public DateTimeOffset Time { get; set; }
        public PredictionRequestModel[] Models { get; set; }
    }
    public class PredictionRequestModel
    {
        public string Output { get; set; }
        public string ModelName { get; set; }
    }

    public class PredictionClassifierResponse
    {
        public int CryptoId { get; set; }
        public DateTimeOffset Time { get; set; }
        public Dictionary<string, decimal> Predictions { get; set; }
    }
}
