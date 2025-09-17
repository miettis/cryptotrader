using CryptoTrader.Data;
using CryptoTrader.Data.Features.Characteristics;
using CryptoTrader.Data.Features.MovingAverages;

namespace CryptoTrader.Web.Models
{
    public abstract class PriceResponse : Price
    {
        
        public string Symbol { get; set; }
        /*
        public DateTimeOffset TimeOpen { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public decimal QuoteVolume { get; set; }
        public long? Trades { get; set; }
        public decimal BuyVolume { get; set; }
        public decimal BuyQuoteVolume { get; set; }
        public decimal MarketCap { get; set; }
        public Average Avg { get; set; } = new Average();
        public Proportion Proportion { get; set; } = new Proportion();
        public Length Length { get; set; } = new Length();
        //public Return Return { get; set; } = new Return();
        */
        public decimal? PredictionHigh { get; set; }
        public decimal? PredictionLow { get; set; }

        protected void Populate(Price price)
        {
            if (Symbol == null)
            {
                Symbol = price.Crypto?.Symbol;
            }
            foreach(var property in typeof(Price).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(price));
                }
            }
        }
    }

    /*
    public class PriceMinuteResponse : PriceResponse
    {
        public MovingAverageMinute MA { get; set; } = new MovingAverageMinute();
        public MovingAverageMinute WMA { get; set; } = new MovingAverageMinute();
        public SlopeMinute Slope { get; set; } = new SlopeMinute();

        public static PriceMinuteResponse Create(PriceMinute price) 
        {
            var result = new PriceMinuteResponse
            {
                MA = price.MA,
                WMA = price.WMA,
                Slope = price.Slope
            };
            result.Populate(price);

            return result;
        }
    }
    */

    public class PriceHourResponse : PriceResponse
    {
        //public SimpleMovingAverage MA { get; set; } = new SimpleMovingAverage();
        //public SlopeHour Slope { get; set; } = new SlopeHour();

        public static PriceHourResponse Create(Price price)
        {
            var result = new PriceHourResponse
            {
                //MA = price.MA?.SMA24 ?? new SimpleMovingAverage(),
                //Slope = price.Trend?.Slope ?? new SlopeHour()
            };
            result.Populate(price);

            return result;
        }
    }
}
