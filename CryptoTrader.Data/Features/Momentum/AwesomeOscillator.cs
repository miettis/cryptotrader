using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class AwesomeOscillator
    {
        public static readonly int DefaultFastPeriods = 6;
        public static readonly int DefaultSlowPeriods = 24;

        [Column("osc")]
        public decimal? Oscillator { get; set; }

        [Column("norm")]
        public decimal? Normalized { get; set; }
    }
}
