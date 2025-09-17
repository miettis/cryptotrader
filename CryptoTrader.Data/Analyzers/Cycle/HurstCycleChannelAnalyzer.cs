using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class HurstCycleChannelAnalyzer : StockDataAnalyzerBase<HurstCycleChannelAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateHurstCycleChannel(settings.MaType, settings.FastLength, settings.SlowLength, settings.FastLength, settings.SlowMult);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Hcc", results.OutputValues["Hcc"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.WildersSmoothingMethod;
            public int FastLength { get; set; } = 10;
            public int SlowLength { get; set; } = 30;
            public double FastMult { get; set; } = 1;
            public double SlowMult { get; set; } = 3;
        }
    }
}
