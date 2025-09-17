using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AtrAnalyzer : SkendrAnalyzerBase<AtrAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAtr(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                { "Atr", result.Select(x => x.Atr).ToList() },
                { "Atrp", result.Select(x => x.Atrp).ToList() },
                { "Tr", result.Select(x => x.Tr).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}
