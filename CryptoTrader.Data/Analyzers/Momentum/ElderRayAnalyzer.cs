using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ElderRayAnalyzer : SkendrAnalyzerBase<ElderRayAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetElderRay(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"BullPower", result.Select(x => x.BullPower).ToList() },
                {"BearPower", result.Select(x => x.BearPower).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 13;
        }
    }
}
