using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class BollingerBandsAnalyzer : SkendrAnalyzerBase<BollingerBandsAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetBollingerBands(settings.LookbackPeriods, settings.StandardDeviations);
            return new Dictionary<string, List<double?>>
            {
                {"UpperBand", result.Select(x => x.UpperBand).ToList() },
                {"PercentB", result.Select(x => x.PercentB).ToList() },
                {"LowerBand", result.Select(x => x.LowerBand).ToList() },
                {"Sma", result.Select(x => x.Sma).ToList() },
                {"Width", result.Select(x => x.Width).ToList() },
                {"ZScore", result.Select(x => x.ZScore).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
            public double StandardDeviations { get; set; } = 2.0;
        }
    }
}
