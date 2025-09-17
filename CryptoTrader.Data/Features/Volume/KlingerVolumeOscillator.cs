using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Oscillators
{
    [Owned]
    public class KlingerVolumeOscillator
    {
        public static readonly int DefaultFastPeriods = 24;
        public static readonly int DefaultSlowPeriods = 48;
        public static readonly int DefaultSignalPeriods = 12;

        [Column("osc")]
        public decimal? Oscillator { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}
