using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.MovingAverages
{
    [Owned]
    public class KaufmansAdaptiveMovingAverage
    {
        public static readonly int DefaultErPeriods = 8;
        public static readonly int DefaultFastPeriods = 2;
        public static readonly int DefaultSlowPeriods = 24;

        [Column("er")]
        public decimal? ER { get; set; }

        [Column("kama")]
        public decimal? Kama { get; set; }
    }
}
