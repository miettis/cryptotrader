using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class AtrTrailingStop
    {
        public static readonly int DefaultLookbackPeriods = 12;
        public static readonly int DefaultMultiplier = 3;
        public static readonly EndType DefaultEndType = EndType.HighLow;

        /// <summary>
        /// ATR Trailing Stop line contains both Upper and Lower segments
        /// </summary>
        [Column("stop")]
        public decimal? AtrStop { get; set; }

        /// <summary>
        /// Upper band only
        /// </summary>
        [Column("buy")]
        public decimal? BuyStop { get; set; }

        /// <summary>
        /// Lower band only
        /// </summary>
        [Column("sell")]
        public decimal? SellStop { get; set; }
    }
}
