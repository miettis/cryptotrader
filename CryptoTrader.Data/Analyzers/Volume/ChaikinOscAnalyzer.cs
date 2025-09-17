using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ChaikinOscAnalyzer : SkendrAnalyzerBase<ChaikinOscAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetChaikinOsc(settings.FastPeriods, settings.SlowPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"MoneyFlowMultiplier", result.Select(x => x.MoneyFlowMultiplier).ToList() },
                {"MoneyFlowVolume", result.Select(x => x.MoneyFlowVolume).ToList() },
                {"Adl", result.Select(x => x.Adl).ToList() },
                {"Oscillator", result.Select(x => x.Oscillator).ToList() }
            };
        }

        public class Settings
        {
            public int FastPeriods { get; set; } = 3;
            public int SlowPeriods { get; set; } = 10;
        }
    }
}
