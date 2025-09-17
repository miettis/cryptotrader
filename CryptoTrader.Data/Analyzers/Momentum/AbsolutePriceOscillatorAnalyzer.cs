using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class AbsolutePriceOscillatorAnalyzer : StockDataAnalyzerBase<AbsolutePriceOscillatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateAbsolutePriceOscillator(settings.MaType, settings.FastLength, settings.SlowLength);
            return results.OutputValues;
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.ExponentialMovingAverage;
            public int FastLength { get; set; } = 10;
            public int SlowLength { get; set; } = 20;
        }
    }
}
