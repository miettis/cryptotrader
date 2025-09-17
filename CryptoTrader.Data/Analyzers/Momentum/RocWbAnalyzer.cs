using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class RocWbAnalyzer : SkendrAnalyzerBase<RocWbAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetRocWb(settings.LookbackPeriods, settings.EmaPeriods, settings.StdDevPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Roc", result.Select(x => x.Roc).ToList() },
                {"LowerBand", result.Select(x => x.LowerBand).ToList() },
                {"UpperBand", result.Select(x => x.UpperBand).ToList() },
                //{"RocEma", result.Select(x => x.RocEma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 12;
            public int EmaPeriods { get; set; } = 3;
            public int StdDevPeriods { get; set; } = 12;
        }
    }
}
