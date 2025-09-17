using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class EmaAnalyzer : SkendrAnalyzerBase<EmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetEma(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Ema", result.Select(x => x.Ema).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}
