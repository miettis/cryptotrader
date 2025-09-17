namespace CryptoTrader.Data.Analyzers.Custom
{
    public class ProfitRankAnalyzer : SecondOrderAnalyzerBase<ProfitRankAnalyzer.Settings>
    {
        public ProfitRankAnalyzer(BinanceContext context) : base(context)
        { 
        }

        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var indicator = GetIndicator<ProfitAnalyzer, ProfitAnalyzer.Settings>(x => x.WindowPeriods == settings.WindowPeriods);

            if (indicator == null) 
            {
                throw new Exception("Required indicator(s) not found");
            }

            var profitFeature = indicator.Features.First(x => x.Output.Key == "Profit");

            if(profitFeature == null)
            {
                throw new Exception("Required feature(s) not found");
            }

            var priceProfits = GetFeatureValuesZip(prices, profitFeature.Id);

            var ranks = new double?[prices.Length];
            for (var i = 0; i < priceProfits.Length - settings.WindowPeriods; i++)
            {
                var current = priceProfits[i];
                var window = priceProfits[i..(i + settings.WindowPeriods)];
                var rank = window.OrderBy(x => x.FeatureValue).SkipWhile(x => x.Price != current.Price).Count();
                ranks[i] = rank;
            }

            return new Dictionary<string, List<double?>>
            {
                { "Rank", ranks.ToList() }
            };
        }

        public override string[] GetOutputs()
        {
            return ["Rank"];
        }
        public class Settings
        {
            public int WindowPeriods { get; set; } = 24;
        }
    }
}
