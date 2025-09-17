using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class PivotsAnalyzer : SkendrAnalyzerBase<PivotsAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetPivots(settings.LeftSpan, settings.RightSpan, settings.MaxTrendPeriods, settings.EndType);
            return new Dictionary<string, List<double?>>
            {
                {"HighLine", result.Select(x => (double?)x.HighLine).ToList() },
                {"HighPoint", result.Select(x => (double?)x.HighPoint).ToList() },
                {"HighTrend", result.Select(x => (double?)x.HighTrend).ToList() },
                {"LowLine", result.Select(x => (double?)x.LowLine).ToList() },
                {"LowPoint", result.Select(x => (double?)x.LowPoint).ToList() },
                {"LowTrend", result.Select(x => (double?)x.LowTrend).ToList() }
            };
        }

        public class Settings
        {
            public int LeftSpan { get; set; } = 2;
            public int RightSpan { get; set; } = 2;
            public int MaxTrendPeriods { get; set; } = 20;
            public EndType EndType { get; set; } = EndType.HighLow;
        }
    }
}
