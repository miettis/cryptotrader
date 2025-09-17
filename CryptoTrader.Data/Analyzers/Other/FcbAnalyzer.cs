using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class FcbAnalyzer : SkendrAnalyzerBase<FcbAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetFcb(settings.WindowSpan);
            return new Dictionary<string, List<double?>>
            {
                {"UpperBand", result.Select(x => (double?)x.UpperBand).ToList() },
                {"LowerBand", result.Select(x => (double?)x.LowerBand).ToList() }
            };
        }

        public class Settings
        {
            public int WindowSpan { get; set; } = 2;
        }
    }
}
