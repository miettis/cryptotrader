using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class T3Analyzer : SkendrAnalyzerBase<T3Analyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetT3(settings.LookbackPeriods, settings.VolumeFactor);
            return new Dictionary<string, List<double?>>
            {
                {"T3", result.Select(x => x.T3).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 5;
            public double VolumeFactor { get; set; } = 0.7;
        }
    }
}
