// HistoricalVolatilityAnalyzer.cs
using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class HistoricalVolatilityAnalyzer : StockDataAnalyzerBase<HistoricalVolatilityAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateHistoricalVolatility(settings.MaType, settings.Length);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Hv", results.OutputValues["Hv"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.ExponentialMovingAverage;
            public int Length { get; set; } = 20;
        }
    }
}
