using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volatility
{
    [Owned]
    public class VolatilityStop
    {
        public static readonly int DefaultLookbackPeriods = 6;
        public static readonly double DefaultMultiplier = 3.0;

        /// <summary>
        /// Stop and Reverse value contains both Upper and Lower segments
        /// </summary>
        [Column("sar")]
        public decimal? SAR { get; set; }

        /// <summary>
        /// Indicates a trend reversal
        /// </summary>
        [Column("stop")]
        public bool IsStop { get; set; }

        /// <summary>
        /// Upper band only (bearish/red)
        /// </summary>
        [Column("upper")]
        public decimal? Upper { get; set; }

        /// <summary>
        /// Lower band only (bullish/green)
        /// </summary>
        [Column("lower")]
        public decimal? Lower { get; set; }
    }
}
