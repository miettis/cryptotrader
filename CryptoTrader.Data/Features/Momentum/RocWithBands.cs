using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class RocWithBands
    {
        public static readonly int DefaultLookbackPeriods = 12;
        public static readonly int DefaultEmaPeriods = 3;
        public static readonly int DefaultStdDevPeriods = 12;


        [Column("roc")]
        public decimal? Roc { get; set; }

        [Column("ema")]
        public decimal? Ema { get; set; }

        [Column("upper")]
        public decimal? Upper { get; set; }

        [Column("lower")]
        public decimal? Lower { get; set; }
    }
}
