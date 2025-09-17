using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volume
{
    [Owned]
    public class ChaikinOscillator
    {
        public static readonly int DefaultFastPeriods = 3;
        public static readonly int DefaultSlowPeriods = 8;

        [Column("mfm")]
        public decimal? MoneyFlowMultiplier { get; set; }

        [Column("mfv")]
        public decimal? MoneyFlowVolume { get; set; }

        [Column("adl")]
        public decimal? Adl { get; set; }

        [Column("osc")]
        public decimal? Oscillator { get; set; }
    }
}
