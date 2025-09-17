namespace CryptoTrader.Data
{
    public class PriceStreak
    {
        public long Id { get; set; }
        public Crypto Crypto { get; set; }
        public int CryptoId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int Count { get; set; }
    }
}
