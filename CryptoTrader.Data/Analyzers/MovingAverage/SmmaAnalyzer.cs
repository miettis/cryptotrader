using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class SmmaAnalyzer : SkendrAnalyzerBase<SmmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetSmma(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Smma", result.Select(x => x.Smma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
