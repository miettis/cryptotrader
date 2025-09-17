using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class FractalAnalyzer : SkendrAnalyzerBase<FractalAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetFractal(settings.WindowSpan, settings.EndType);
            return new Dictionary<string, List<double?>>
            {
                {"FractalBear", result.Select(x => (double?)x.FractalBear).ToList() },
                {"FractalBull", result.Select(x => (double?)x.FractalBull).ToList() }
            };
        }

        public class Settings
        {
            public int WindowSpan { get; set; } = 2;
            public EndType EndType { get; set; } = EndType.HighLow;
        }
    }
}
