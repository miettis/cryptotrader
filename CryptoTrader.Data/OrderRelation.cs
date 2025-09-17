namespace CryptoTrader.Data
{
    public class OrderRelation
    {
        public int Id { get; set; }

        public int BuyOrderId { get; set; }
        public Order BuyOrder { get; set; }

        public int SellOrderId { get; set; }
        public Order SellOrder { get; set; }

        public decimal Quantity { get; set; }
    }
}
