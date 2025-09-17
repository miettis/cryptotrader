using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class UlcerIndexAnalyzer : SkendrAnalyzerBase<UlcerIndexAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetUlcerIndex(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"UI", result.Select(x => x.UI).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}
