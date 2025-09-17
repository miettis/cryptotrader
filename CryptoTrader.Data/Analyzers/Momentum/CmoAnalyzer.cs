using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class CmoAnalyzer : SkendrAnalyzerBase<CmoAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetCmo(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Cmo", result.Select(x => x.Cmo).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
