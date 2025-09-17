using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class SuperTrend
    {
        public static readonly int DefaultLookbackPeriods = 8;
        public static readonly double DefaultMultiplier = 3;

        /// <summary>
        /// SuperTrend line contains both Upper and Lower segments
        /// </summary>
        [Column("combined")]
        public decimal? Combined { get; set; }

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
