using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class CciAnalyzer : SkendrAnalyzerBase<CciAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetCci(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Cci", result.Select(x => x.Cci).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
