using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ChandelierAnalyzer : SkendrAnalyzerBase<ChandelierAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetChandelier(settings.LookbackPeriods, settings.Multiplier);
            return new Dictionary<string, List<double?>>
            {
                {"ChandelierExit", result.Select(x => x.ChandelierExit).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 22;
            public double Multiplier { get; set; } = 3.0;
            public ChandelierType ChandelierType { get; set; } = ChandelierType.Long;
        }
    }
}
