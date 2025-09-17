namespace CryptoTrader.Data.Analyzers.Custom
{
    public class MacdCrossoverAnalyzer : SecondOrderAnalyzerBase<MacdCrossoverAnalyzer.Settings>
    {
        public MacdCrossoverAnalyzer(BinanceContext context) : base(context)
        { 
        }

        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var indicator = GetIndicator<MacdAnalyzer>(settings.MacdIndicatorId);

            if (indicator == null) 
            {
                throw new Exception("Required indicator(s) not found");
            }

            var macdFeature = indicator.Features.First(x => x.Output.Key == "Macd");
            var signalFeature = indicator.Features.First(x => x.Output.Key == "Signal");

            if(macdFeature == null || signalFeature == null)
            {
                throw new Exception("Required feature(s) not found");
            }

            var macds = GetFeatureValues(prices, macdFeature.Id);
            var signals = GetFeatureValues(prices, signalFeature.Id);

            var crossUp = new double?[prices.Length];
            var crossDown = new double?[prices.Length];

            for (var i = 1; i < prices.Length; i++)
            {
                var prev = prices[i - 1];
                var current = prices[i];

                if (!macds.TryGetValue(current.Id, out var macdCurrent) || 
                    !signals.TryGetValue(current.Id, out var signalCurrent) ||
                    !macds.TryGetValue(prev.Id, out var macdPrev) ||
                    !signals.TryGetValue(prev.Id, out var signalPrev))
                {
                    crossUp[i] = 0;
                    crossDown[i] = 0;
                    continue;
                }

                if(macdPrev < signalPrev && macdCurrent > signalCurrent)
                {
                    crossUp[i] = 1; 
                    crossDown[i] = 0;
                }
                else if (macdPrev > signalPrev && macdCurrent < signalCurrent)
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
            public int MacdIndicatorId { get; set; }
        }
    }
}
