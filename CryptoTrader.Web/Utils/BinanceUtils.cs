using Binance.Net.Interfaces;
using CryptoTrader.Data;
using Microsoft.IdentityModel.Tokens;

namespace CryptoTrader.Web.Utils
{
    public static class BinanceUtils
    {
        public static T ToPrice<T>(this IBinanceKline kline) where T : Price, new()
        {
            var price = new T
            {
                TimeOpen = kline.OpenTime,
                Open = kline.OpenPrice,
                High = kline.HighPrice,
                Low = kline.LowPrice,
                Close = kline.ClosePrice,
                Volume = kline.Volume,
                QuoteVolume = kline.QuoteVolume,
                BuyVolume = kline.TakerBuyBaseVolume,
                BuyQuoteVolume = kline.TakerBuyQuoteVolume,
                Trades = kline.TradeCount
            };
            price.Timestamp = price.TimeOpen.ToUnixTimeMilliseconds();
            price.PopulateCalculatedValues();

            return price;
        }
    }
}
