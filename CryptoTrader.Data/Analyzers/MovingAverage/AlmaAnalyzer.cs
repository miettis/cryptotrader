using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AlmaAnalyzer : SkendrAnalyzerBase<AlmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAlma(settings.LookbackPeriods, settings.Offset, settings.Sigma);
            return new Dictionary<string, List<double?>>
            {
                {"Alma", result.Select(x => x.Alma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 9;
            public double Offset { get; set; } = 0.85;
            public double Sigma { get; set; } = 6;
        }
    }
}
