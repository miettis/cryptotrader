using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class VwmaAnalyzer : SkendrAnalyzerBase<VwmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetVwma(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Vwma", result.Select(x => x.Vwma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
