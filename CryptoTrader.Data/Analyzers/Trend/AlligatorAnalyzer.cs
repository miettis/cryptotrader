using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AlligatorAnalyzer : SkendrAnalyzerBase<AlligatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAlligator(settings.JawPeriods, settings.JawOffset, settings.TeethPeriods, settings.TeethOffset, settings.LipsPeriods, settings.LipsOffset);
            return new Dictionary<string, List<double?>>
            {
                {"Jaw", result.Select(x => x.Jaw).ToList() },
                {"Lips", result.Select(x => x.Lips).ToList() },
                {"Teeth", result.Select(x => x.Teeth).ToList() }
            };
        }

        public class Settings
        {
            public int JawPeriods { get; set; } = 13;
            public int JawOffset { get; set; } = 8;
            public int TeethPeriods { get; set; } = 8;
            public int TeethOffset { get; set; } = 5;
            public int LipsPeriods { get; set; } = 5;
            public int LipsOffset { get; set; } = 3;
        }
    }
}
