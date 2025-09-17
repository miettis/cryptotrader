using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ParabolicSarAnalyzer : SkendrAnalyzerBase<ParabolicSarAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetParabolicSar(settings.AccelerationStep, settings.MaxAccelerationFactor, settings.InitialFactor);
            return new Dictionary<string, List<double?>>
            {
                // TODO: map boolean to double?
                //{"IsReversal", result.Select(x => x.IsReversal).ToList() }
                {"Sar", result.Select(x => x.Sar).ToList() }
            };
        }

        public class Settings
        {
            public double AccelerationStep { get; set; } = 0.02;
            public double MaxAccelerationFactor { get; set; } = 0.2;
            public double InitialFactor { get; set; } = 0.02;

        }
    }
}
