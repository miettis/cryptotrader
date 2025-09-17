using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class TsiAnalyzer : SkendrAnalyzerBase<TsiAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetTsi(settings.LookbackPeriods, settings.SmoothPeriods, settings.SignalPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Tsi", result.Select(x => x.Tsi).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 25;
            public int SmoothPeriods { get; set; } = 13;
            public int SignalPeriods { get; set; } = 7;
        }
    }
}
