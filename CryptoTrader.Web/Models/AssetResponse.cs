using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Data.Features;
using CryptoTrader.Data.Features.MovingAverages;

namespace CryptoTrader.Web.Models
{
    public class AssetResponse
    {
        public string Symbol { get; set; }
        public bool Followed { get; set; }
        public bool Trade { get; set; }
        public decimal Available { get; set; }
        public decimal Total { get; set; }
        public decimal ValueLow { get;  set; }
        public decimal ValueHigh { get; set; }
        public decimal ValueAvg { get; set; }
        public decimal ValueMA24 { get; set; }
        public decimal ValueMA24Upper { get; set; }
        public decimal ValueMA24Lower { get; set; }
        public decimal? LatestPurchasePrice { get; set; }
        public SimpleMovingAverage MA { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public static AssetResponse Create(string symbol, Crypto? crypto, Price? price, BinanceBalance? asset)
        {
            var result = new AssetResponse
            {
                Symbol = symbol,
                Available = asset?.Available ?? 0m,
                Total = asset?.Total ?? 0m
            };
            if (price != null)
            {
                result.High = price.High;
                result.Low = price.Low;
                result.MA = price.MA?.SMA12;
                result.ValueLow = Math.Round(price.Low * result.Total, 2);
                result.ValueHigh = Math.Round(price.High * result.Total, 2);
                result.ValueAvg = Math.Round(price.Avg.HL * result.Total, 2);
                if (price.MA?.SMA24?.Sma != null)
                {
                    result.ValueMA24 = Math.Round(price.MA.SMA24.Sma.Value * result.Total, 2);
                    result.ValueMA24Upper = Math.Round((price.MA.SMA24.Sma.Value + price.MA.SMA24.Mad.Value) * result.Total, 2);
                    result.ValueMA24Lower = Math.Round((price.MA.SMA24.Sma.Value - price.MA.SMA24.Mad.Value) * result.Total, 2);
                }
            }
            if(crypto != null)
            {
                result.Trade = crypto.Trade;
                result.Followed = crypto.Followed.HasValue;
            }
            
            if (symbol == AssetExtensions.QuoteAsset)
            {
                result.ValueLow = result.Total;
                result.ValueHigh = result.Total;
                result.ValueAvg = result.Total;
                result.ValueMA24 = result.Total;
                result.ValueMA24Upper = result.Total;
                result.ValueMA24Lower = result.Total;
            }
            
            return result;
        }
    }
}
