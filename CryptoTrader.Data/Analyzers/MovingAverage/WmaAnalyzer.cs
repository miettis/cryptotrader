using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class WmaAnalyzer : SkendrAnalyzerBase<WmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetWma(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Wma", result.Select(x => x.Wma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
