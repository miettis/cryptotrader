using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class TrueStrengthIndex
    {
        public static readonly int DefaultLookbackPeriods = 24;
        public static readonly int DefaultSmoothPeriods = 12;
        public static readonly int DefaultSignalPeriods = 6;

        [Column("tsi")]
        public decimal? Tsi { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}
