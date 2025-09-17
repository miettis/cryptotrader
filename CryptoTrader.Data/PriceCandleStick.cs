using CryptoTrader.Data.Features.CandleSticks;

namespace CryptoTrader.Data
{
    public class PriceCandleStick
    {
        public int CryptoId { get; set; }
        public DateTimeOffset LastTime { get; set; }
        public int Length { get; set; }
        public CandleStickPattern Pattern { get; set; }
    }
}
