using System.ComponentModel.DataAnnotations;

namespace CryptoTrader.Data
{
    public class Order
    {
        public int Id { get; set; }

        public long BinanceId { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public OrderStatus Status { get; set; }

        public OrderSide Side { get; set; }

        [MaxLength(10)]
        public required string Symbol { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public decimal? ExecutedQuantity { get; set; }
        public decimal? QuoteQuantity { get; set; }
        public decimal? AverageFillPrice { get; set; }
        public decimal? Commission { get; set; }
        public decimal? RemainingQuantity { get; set; }

        public string? CreateResponse { get; set; }

        public string? CancelResponse { get; set; }

        /// <summary>
        /// Not yet sold
        /// </summary>
        public decimal? UnmatchedQuantity { get; set; }

        public List<OrderRelation> SellOrders { get; set; }
        public List<OrderRelation> BuyOrders { get; set; }
    }
}
