using CryptoTrader.Data.Features;
using CryptoTrader.Data.Features.Characteristics;
using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data
{
    public class Price : IQuote
    {
        public long Id { get; set; }

        [JsonIgnore]
        public Crypto Crypto { get; set; }

        public int CryptoId { get; set; }

        [JsonIgnore]
        public long Timestamp { get; set; }

        public DateTimeOffset TimeOpen { get; set; }

        [JsonIgnore]
        DateTime ISeries.Date => TimeOpen.DateTime;

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        [Precision(38, 18)]
        public decimal Volume { get; set; }

        [Precision(38, 18)]
        public decimal QuoteVolume { get; set; }

        public long? Trades { get; set; }

        [Precision(38, 18)]
        public decimal BuyVolume { get; set; }

        [Precision(38, 18)]
        public decimal BuyQuoteVolume { get; set; }

        [Precision(38, 18)]
        public decimal MarketCap { get; set; }

        public Average Avg { get; set; } = new Average();

        public Proportion Proportion { get; set; } = new Proportion();

        public Length Length { get; set; } = new Length();

        [JsonIgnore]
        public PriceCandleSticks? CandleSticks { get; set; }
        public PriceCycles? Cycles { get; set; }
        public PriceMovingAverages? MA { get; set; }
        public PriceMomentums Momentum { get; set; }
        public PriceOtherIndicators? OtherIndicators { get; set; }
        public PricePeak? Peak { get; set; }
        public PriceReturn? Return { get; set; }
        public PriceTrends? Trend { get; set; }
        public PriceVolatilities Volatility { get; set; }
        public PriceVolumes? Volumes { get; set; }
        public List<PriceFeature> Features { get; set; }

        public void PopulateCalculatedValues()
        {
            Avg = new Average();
            Proportion = new Proportion();
            Length = new Length();

            Avg.OHLC = (Open + High + Low + Close) / 4;
            Avg.HLC = (High + Low + Close) / 3;
            Avg.HL = (High + Low) / 2;
            Avg.OC = (Open+Close) / 2;

            var diff = High - Low;
            var max = new[] { Open, High, Low, Close }.Max();
            if (diff > 0 && High == max)
            {
                Proportion.Upper = (High - Math.Max(Open, Close)) / diff;
                Proportion.Body = Math.Abs(Open - Close) / diff;
                Proportion.Lower = (Math.Min(Open, Close) - Low) / diff;
            }
            else
            {
                Proportion.Upper = 1;
                Proportion.Body = 1;
                Proportion.Lower = 1;
            }

            if (High > 0)
            {
                Length.Candle = (High - Low) / High;
                Length.Upper = (High - Math.Max(Open, Close)) / High;
                Length.Body = Math.Abs(Open - Close) / High;
                Length.Lower = (Math.Min(Open, Close) - Low) / High;
            }
        }

        
        public decimal? GetSlope()
        {
            if(Trend?.Slope.High8 != null && Trend?.Slope.Low8 != null)
            {
                return (Trend.Slope.High8.Value + Trend.Slope.Low8.Value) / 2;
            }
            
            return null;
        }

    }
}
