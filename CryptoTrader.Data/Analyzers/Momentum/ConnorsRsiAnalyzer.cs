using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ConnorsRsiAnalyzer : SkendrAnalyzerBase<ConnorsRsiAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetConnorsRsi(settings.RsiPeriods, settings.StreakPeriods, settings.RankPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"ConnorsRsi", result.Select(x => x.ConnorsRsi).ToList() },
                {"PercentRank", result.Select(x => x.PercentRank).ToList() },
                {"Rsi", result.Select(x => x.Rsi).ToList() },
                {"RsiStreak", result.Select(x => x.RsiStreak).ToList() }
            };
        }

        public class Settings
        {
            public int RsiPeriods { get; set; } = 3;
            public int StreakPeriods { get; set; } = 2;
            public int RankPeriods { get; set; } = 100;
        }
    }
}
