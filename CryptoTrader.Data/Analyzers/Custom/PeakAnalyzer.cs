namespace CryptoTrader.Data.Analyzers.Custom
{
    public class PeakAnalyzer : CustomAnalyzerBase<PeakAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var highestHigh = new double?[prices.Length];
            var lowestLow = new double?[prices.Length];

            for (var i = 0; i < prices.Length; i++)
            {
                var current = prices[i];

                var hasHigherHighPrev = false;
                var hasLowerLowPrev = false;
                var hasHigherHighNext = false;
                var hasLowerLowNext = false;

                for (var offset = 1; offset <= settings.WindowPeriods; offset++)
                {
                    if (i - offset >= 0)
                    {
                        var prev = prices[i - offset];
                        if (prev.High > current.High)
                        {
                            hasHigherHighPrev = true;
                        }
                        if (prev.Low < current.Low)
                        {
                            hasLowerLowPrev = true;
                        }
                    }

                    if ((i + offset) < prices.Length)
                    {
                        var next = prices[i + offset];
                        if (next.High > current.High)
                        {
                            hasHigherHighNext = true;
                        }
                        if (next.Low < current.Low)
                        {
                            hasLowerLowNext = true;
                        }
                    }
                }

                if(!hasHigherHighPrev && !hasHigherHighNext)
                {
                    highestHigh[i] = 1;
                }
                else
                {
                    highestHigh[i] = 0;
                }

                if (!hasLowerLowPrev && !hasLowerLowNext)
                {
                    lowestLow[i] = 1;
                }
                else
                {
                    lowestLow[i] = 0;
                }
            }

            var offsetHighestHigh = new List<double?>(prices.Length);
            Price prevHH = null;
            Price prevLL = null;
            for (var i = 0; i < prices.Length; i++)
            {
                var current = prices[i];
                
            }

            return new Dictionary<string, List<double?>>
            {
                { "HighestHigh", highestHigh.ToList()},
                { "LowestLow", lowestLow.ToList() }
            };
        }
        public override string[] GetOutputs()
        {
            return ["HighestHigh", "LowestLow"];
        }
        public class Settings
        {
            public int WindowPeriods { get; set; }
        }
    }
}
