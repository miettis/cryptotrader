using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class PercentagePriceOscillatorAnalyzer : StockDataAnalyzerBase<PercentagePriceOscillatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculatePercentagePriceOscillator(settings.MaType, settings.FastLength, settings.SlowLength, settings.SignalLength);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Ppo", results.OutputValues["Ppo"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.ExponentialMovingAverage;
            public int FastLength { get; set; } = 12;
            public int SlowLength { get; set; } = 26;
            public int SignalLength { get; set; } = 9;
        }
    }
}
