using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class StochasticMomentumIndex
    {
        public static readonly int DefaultLookbackPeriods = 12;
        public static readonly int DefaultFirstSmoothPeriods = 24;
        public static readonly int DefaultSecondSmoothPeriods = 2;
        public static readonly int DefaultSignalPeriods = 3;

        [Column("smi")]
        public decimal? Smi { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}
