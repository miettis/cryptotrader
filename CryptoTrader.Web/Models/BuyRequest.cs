namespace CryptoTrader.Web.Models
{
    public class BuyRequest
    {
        public string Symbol { get; set; }
        public BuyType Type { get; set; }
        public decimal USDT { get; set; }
    }
    public enum BuyType
    {
        Low,
        MA24Std
    }
}
