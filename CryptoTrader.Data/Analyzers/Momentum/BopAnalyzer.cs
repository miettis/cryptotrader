using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class BopAnalyzer : SkendrAnalyzerBase<BopAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetBop(settings.SmoothPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Bop", result.Select(x => x.Bop).ToList() }
            };
        }

        public class Settings 
        {
            public int SmoothPeriods { get; set; } = 14;
        }
    }
}
