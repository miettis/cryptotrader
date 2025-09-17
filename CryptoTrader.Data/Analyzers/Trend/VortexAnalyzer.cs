using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class VortexAnalyzer : SkendrAnalyzerBase<VortexAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetVortex(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Nvi", result.Select(x => x.Nvi).ToList() },
                {"Pvi", result.Select(x => x.Pvi).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}
