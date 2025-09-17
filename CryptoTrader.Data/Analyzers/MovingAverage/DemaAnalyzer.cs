using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class DemaAnalyzer : SkendrAnalyzerBase<DemaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetDema(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Dema", result.Select(x => x.Dema).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
