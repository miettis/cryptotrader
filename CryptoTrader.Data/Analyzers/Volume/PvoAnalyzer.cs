using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class PvoAnalyzer : SkendrAnalyzerBase<PvoAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetPvo(settings.FastPeriods, settings.SlowPeriods, settings.SignalPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Pvo", result.Select(x => x.Pvo).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() },
                {"Histogram", result.Select(x => x.Histogram).ToList() }
            };
        }

        public class Settings
        {
            public int FastPeriods { get; set; } = 12;
            public int SlowPeriods { get; set; } = 26;
            public int SignalPeriods { get; set; } = 9;
        }
    }
}
