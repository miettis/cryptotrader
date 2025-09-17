using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class PivotPointsAnalyzer : SkendrAnalyzerBase<PivotPointsAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetPivotPoints(settings.WindowSize, settings.PivotPointType);
            return new Dictionary<string, List<double?>>
            {
                {"PP", result.Select(x => (double?)x.PP).ToList() },
                {"R1", result.Select(x => (double?)x.R1).ToList() },
                {"R2", result.Select(x => (double?)x.R2).ToList() },
                {"R3", result.Select(x => (double?)x.R3).ToList() },
                //{"R4", result.Select(x => (double?)x.R4).ToList() },
                {"S1", result.Select(x => (double?)x.S1).ToList() },
                {"S2", result.Select(x => (double?)x.S2).ToList() },
                {"S3", result.Select(x => (double?)x.S3).ToList() },
                //{"S4", result.Select(x => (double?)x.S4).ToList() }
            };
        }

        public class Settings
        {
            public PeriodSize WindowSize { get; set; } = PeriodSize.Day;
            public PivotPointType PivotPointType { get; set; } = PivotPointType.Standard;
        }
    }
}
