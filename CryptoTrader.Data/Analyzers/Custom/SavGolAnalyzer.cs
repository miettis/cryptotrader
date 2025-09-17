using CryptoTrader.Data.Services;

namespace CryptoTrader.Data.Analyzers.Custom
{
    public class SavGolAnalyzer : CustomAnalyzerBase<SavGolAnalyzer.Settings>
    {
        private readonly PythonService predictionService;
        public SavGolAnalyzer(PythonService pythonService)
        {
            predictionService = pythonService;
        }
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            // TODO: needs to be calculated with information available by that time
            var request = new SavgolRequest
            {
                Data =
                    [
                        new SavgolRequestValues { Field = nameof(Price.High), Values = prices.Select(x => x.High).ToArray() },
                        new SavgolRequestValues { Field = nameof(Price.Low), Values = prices.Select(x => x.Low).ToArray() },
                        new SavgolRequestValues { Field = nameof(Price.Avg), Values = prices.Select(x => (x.High + x.Low) / 2).ToArray() },
                    ],
                Order = settings.Order,
                Windows = [settings.WindowPeriods]
            };
            var response = predictionService.GetSavgolV2(request).Result;

            var result = new Dictionary<string, List<double?>>();

            var highValues = response.FirstOrDefault(x => x.Field == nameof(Price.High));
            if (highValues != null)
            {
                result.Add("HighSmooth", highValues.Smooth.Select(x => (double?)x).ToList());
                result.Add("HighSlope", highValues.Derivatives.Select(x => (double?)x).ToList());
            }
            var lowValues = response.FirstOrDefault(x => x.Field == nameof(Price.Low));
            if (lowValues != null)
            {
                result.Add("LowSmooth", lowValues.Smooth.Select(x => (double?)x).ToList());
                result.Add("LowSlope", lowValues.Derivatives.Select(x => (double?)x).ToList());
            }
            var avgValues = response.FirstOrDefault(x => x.Field == nameof(Price.Avg));
            if (avgValues != null)
            {
                result.Add("AvgSmooth", avgValues.Smooth.Select(x => (double?)x).ToList());
                result.Add("AvgSlope", avgValues.Derivatives.Select(x => (double?)x).ToList());
            }

            return result;
        }
        public override string[] GetOutputs()
        {
            return ["HighSmooth", "HighSlope", "LowSmooth", "LowSlope", "AvgSmooth", "AvgSlope"];
        }
        public class Settings
        {
            public int WindowPeriods { get; set; } = 24;
            public int Order { get; set; } = 3;
        }
    }
}
