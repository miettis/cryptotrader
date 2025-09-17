using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class MovingAverageConvergenceDivergence
    {
        public static readonly int DefaultFastPeriods = 12;
        public static readonly int DefaultSlowPeriods = 24;
        public static readonly int DefaultSignalPeriods = 9;

        /// <summary>
        /// The MACD line is the difference between slow and fast moving averages
        /// </summary>
        [Column("macd")]
        public decimal? Macd { get; set; }

        /// <summary>
        /// Moving average of the MACD line
        /// </summary>
        [Column("signal")]
        public decimal? Signal { get; set; }

        /// <summary>
        /// Gap between of the MACD and Signal line
        /// </summary>
        [Column("hist")]
        public decimal? Histogram { get; set; }

        // EMA already in separate indicator
        /*
        public decimal? FastEma { get; set; }
        public decimal? SlowEma { get; set; }
        */
    }
}
