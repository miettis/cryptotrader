using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class KvoAnalyzer : SkendrAnalyzerBase<KvoAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetKvo(settings.FastPeriods, settings.SlowPeriods, settings.SignalPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Oscillator", result.Select(x => x.Oscillator).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() }
            };
        }

        public class Settings
        {
            public int FastPeriods { get; set; } = 34;
            public int SlowPeriods { get; set; } = 55;
            public int SignalPeriods { get; set; } = 13;
        }
    }
}
