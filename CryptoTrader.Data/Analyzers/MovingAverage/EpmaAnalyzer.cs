using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class EpmaAnalyzer : SkendrAnalyzerBase<EpmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetEpma(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Epma", result.Select(x => x.Epma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
