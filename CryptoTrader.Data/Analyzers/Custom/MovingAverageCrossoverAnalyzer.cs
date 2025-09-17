namespace CryptoTrader.Data.Analyzers.Custom
{
    public class MovingAverageCrossoverAnalyzer : SecondOrderAnalyzerBase<MovingAverageCrossoverAnalyzer.Settings>
    {
        public MovingAverageCrossoverAnalyzer(BinanceContext context) : base(context)
        { 
        }

        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var shortIndicator = GetIndicator<SmaAnalyzer>(settings.ShortIndicatorId);
            var longIndicator = GetIndicator<SmaAnalyzer>(settings.LongIndicatorId);

            if (shortIndicator == null || longIndicator == null) 
            {
                throw new Exception("Required indicator(s) not found");
            }

            var shortFeature = shortIndicator.Features.First(x => x.Output.Key == settings.OutputKey);
            var longFeature = longIndicator.Features.First(x => x.Output.Key == settings.OutputKey);
            
            if(shortFeature == null || longFeature == null)
            {
                throw new Exception("Required feature(s) not found");
            }

            var shortSmas = GetFeatureValues(prices, shortFeature.Id);
            var longSmas = GetFeatureValues(prices, longFeature.Id);

            var crossUp = new double?[prices.Length];
            var crossDown = new double?[prices.Length];

            for (var i = 1; i < prices.Length; i++)
            {
                var prev = prices[i - 1];
                var current = prices[i];

                if (!shortSmas.TryGetValue(current.Id, out var shortSmaCurrent) ||
                    !longSmas.TryGetValue(current.Id, out var longSmaCurrent) ||
                    !shortSmas.TryGetValue(prev.Id, out var shortSmaPrev) ||
                    !longSmas.TryGetValue(prev.Id, out var longSmaPrev))
                {
                    crossUp[i] = 0;
                    crossDown[i] = 0;
                    continue;
                }

                if(shortSmaPrev < longSmaPrev && shortSmaCurrent > longSmaCurrent)
                {
                    crossUp[i] = 1; 
                    crossDown[i] = 0;
                }
                else if (shortSmaPrev > longSmaPrev && shortSmaCurrent < longSmaCurrent)
                {
                    crossUp[i] = 0;
                    crossDown[i] = 1;
                }
                else
                {
                    crossUp[i] = 0;
                    crossDown[i] = 0;
                }
            }

            return new Dictionary<string, List<double?>>
            {
                { "CrossAbove", crossUp.ToList() },
                { "CrossBelow", crossDown.ToList() }
            };
        }
        public override string[] GetOutputs()
        {
            return ["CrossAbove", "CrossBelow"];
        }
        public class Settings
        {
            public string OutputKey { get; set; }
            public int ShortIndicatorId { get; set; }
            public int LongIndicatorId { get; set; }
        }
    }
}
