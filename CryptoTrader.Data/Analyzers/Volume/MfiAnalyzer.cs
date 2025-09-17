using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class MfiAnalyzer : SkendrAnalyzerBase<MfiAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetMfi(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Mfi", result.Select(x => x.Mfi).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}
