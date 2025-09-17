using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class MaEnvelopesAnalyzer : SkendrAnalyzerBase<MaEnvelopesAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetMaEnvelopes(settings.LookbackPeriods, settings.PercentOffset, settings.MaType);
            return new Dictionary<string, List<double?>>
            {
                {"LowerEnvelope", result.Select(x => x.LowerEnvelope).ToList() },
                {"UpperEnvelope", result.Select(x => x.UpperEnvelope).ToList() },
                {"Centerline", result.Select(x => x.Centerline).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
            public double PercentOffset { get; set; } = 2.5;
            public MaType MaType { get; set; } = MaType.SMA;
        }
    }
}
