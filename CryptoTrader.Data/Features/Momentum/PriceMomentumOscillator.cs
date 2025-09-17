using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class PriceMomentumOscillator
    {
        public static readonly int DefaultTimePeriods = 24;
        public static readonly int DefaultSmoothPeriods = 12;
        public static readonly int DefaultSignalPeriods = 6;

        [Column("pmo")]
        public decimal? Pmo { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}
