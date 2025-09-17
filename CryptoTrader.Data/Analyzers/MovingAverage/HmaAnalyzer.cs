using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class HmaAnalyzer : SkendrAnalyzerBase<HmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetHma(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Hma", result.Select(x => x.Hma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
