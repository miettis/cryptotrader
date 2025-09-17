using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class ChandelierExit
    {
        public static readonly int DefaultLookbackPeriods = 12;
        public static readonly int DefaultMultiplier = 3;

        /// <summary>
        /// Short exit line
        /// </summary>
        [Column("short")]
        public decimal? Short { get; set; }

        /// <summary>
        /// Long exit line
        /// </summary>
        [Column("long")]
        public decimal? Long { get; set; }
    }
}
