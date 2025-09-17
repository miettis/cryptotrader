using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class StochasticFastOscillatorAnalyzer : StockDataAnalyzerBase<StochasticFastOscillatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateStochasticFastOscillator(settings.MaType, settings.Length, settings.SmoothLength1, settings.SmoothLength2);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Sfo", results.OutputValues["Sfo"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.ExponentialMovingAverage;
            public int Length { get; set; } = 14;
            public int SmoothLength1 { get; set; } = 3;
            public int SmoothLength2 { get; set; } = 2;
        }
    }
}
