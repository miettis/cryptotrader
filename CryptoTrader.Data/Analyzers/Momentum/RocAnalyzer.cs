using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class RocAnalyzer : SkendrAnalyzerBase<RocAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetRoc(settings.LookbackPeriods, settings.SmaPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Roc", result.Select(x => x.Roc).ToList() },
                {"Momentum", result.Select(x => x.Momentum).ToList() },
                //{"RocSma", result.Select(x => x.RocSma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 12;
            public int? SmaPeriods { get; set; }
        }
    }
}
