namespace CryptoTrader.Web.Models
{
    public class SellRequest
    {
        public string Symbol { get; set; }
        public SellType Type { get; set; }
    }
    public enum SellType
    {
        High,
        MA24Std,
        DefaultProfit
    }
}
