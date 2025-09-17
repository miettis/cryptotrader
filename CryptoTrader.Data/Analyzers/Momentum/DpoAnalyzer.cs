using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class DpoAnalyzer : SkendrAnalyzerBase<DpoAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetDpo(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Dpo", result.Select(x => x.Dpo).ToList() },
                //{"Sma", result.Select(x => x.Sma).ToList() },
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
