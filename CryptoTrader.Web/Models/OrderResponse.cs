using CryptoTrader.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CryptoTrader.Web.Models
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public long BinanceId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
        public required string Side { get; set; }
        public required string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal? ExecutedQuantity { get; set; }
        public decimal? QuoteQuantity { get; set; }
        public decimal? AverageFillPrice { get; set; }
        public decimal? Commission { get; set; }
        public decimal? QuantityRemaining { get; set; }
        public decimal? Profit { get; set; }

        internal static OrderResponse Create(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                BinanceId = order.BinanceId,
                Created = order.Created,
                Updated = order.Updated ?? order.Created,
                Status = order.Status,
                Side = order.Side.ToString(),
                Symbol = order.Symbol,
                Price = order.Price,
                Quantity = order.Quantity,
                ExecutedQuantity = order.ExecutedQuantity,
                QuoteQuantity = order.QuoteQuantity,
                AverageFillPrice = order.AverageFillPrice,
                Commission = order.Commission,
                QuantityRemaining = order.RemainingQuantity
            };
        }
    }
}
