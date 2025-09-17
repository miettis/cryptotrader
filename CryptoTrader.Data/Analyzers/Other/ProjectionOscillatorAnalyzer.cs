using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class ProjectionOscillatorAnalyzer : StockDataAnalyzerBase<ProjectionOscillatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateProjectionOscillator(settings.MaType, settings.Length, settings.SmoothLength);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Po", results.OutputValues["Po"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.WeightedMovingAverage;
            public int Length { get; set; } = 14;
            public int SmoothLength { get; set; } = 4;
        }
    }
}

