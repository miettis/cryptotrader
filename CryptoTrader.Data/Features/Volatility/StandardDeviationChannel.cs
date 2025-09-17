using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volatility
{
    [Owned]
    public class StandardDeviationChannel
    {
        public static readonly int DefaultLookbackPeriods = 12;
        public static readonly double DefaultStandardDeviations = 2.0;


        [Column("upper")]
        public decimal? Upper { get; set; }

        [Column("center")]
        public decimal? Center { get; set; }

        [Column("lower")]
        public decimal? Lower { get; set; }

        [Column("bp")]
        public bool BreakPoint { get; set; }
    }
}
