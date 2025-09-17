using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class DynamicAnalyzer : SkendrAnalyzerBase<DynamicAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetDynamic(settings.LookbackPeriods, settings.KFactor);
            return new Dictionary<string, List<double?>>
            {
                {"Dynamic", result.Select(x => x.Dynamic).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
            public double KFactor { get; set; } = 0.6;
        }
    }
}
