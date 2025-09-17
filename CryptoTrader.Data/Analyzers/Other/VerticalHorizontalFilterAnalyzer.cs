// VerticalHorizontalFilterAnalyzer.cs
using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class VerticalHorizontalFilterAnalyzer : StockDataAnalyzerBase<VerticalHorizontalFilterAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateVerticalHorizontalFilter(settings.MaType, settings.Length, settings.SignalLength);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Vhf", results.OutputValues["Vhf"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.WeightedMovingAverage;
            public int Length { get; set; } = 18;
            public int SignalLength { get; set; } = 6;
        }
    }
}

