using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class IchimokuAnalyzer : SkendrAnalyzerBase<IchimokuAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetIchimoku(settings.TenkanPeriods, settings.KijunPeriods, settings.SenkouBPeriods, settings.SenkouOffset, settings.ChikouOffset);
            return new Dictionary<string, List<double?>>
            {
                {"TenkanSen", result.Select(x => (double?)x.TenkanSen).ToList() },
                {"KijunSen", result.Select(x => (double?)x.KijunSen).ToList() },
                {"SenkouSpanA", result.Select(x => (double?)x.SenkouSpanA).ToList() },
                {"SenkouSpanB", result.Select(x => (double?)x.SenkouSpanB).ToList() },
                {"ChikouSpan", result.Select(x => (double?)x.ChikouSpan).ToList() }
            };
        }

        public class Settings
        {
            public int TenkanPeriods { get; set; } = 9;
            public int KijunPeriods { get; set; } = 26;
            public int SenkouBPeriods { get; set; } = 52;
            public int SenkouOffset { get; set; } = 26;
            public int ChikouOffset { get; set; } = 26;
        }
    }
}
