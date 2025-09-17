using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Data.Features.Volatility
{
    [Owned]
    public class AverageTrueRange
    {
        /// <summary>
        /// True Range for current period
        /// </summary>
        public decimal? TR { get; set; }

        /// <summary>
        /// Average True Range
        /// </summary>
        public decimal? Atr { get; set; }

        /// <summary>
        /// Average True Range Percent is (ATR/Price)*100. This normalizes so it can be compared to other stocks.
        /// </summary>
        public decimal? Atrp { get; set; }
    }
}
