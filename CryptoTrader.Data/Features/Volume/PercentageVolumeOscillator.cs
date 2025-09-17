using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Oscillators
{
    [Owned]
    public class PercentageVolumeOscillator
    {
        public static readonly int DefaultFastPeriods = 12;
        public static readonly int DefaultSlowPeriods = 24;
        public static readonly int DefaultSignalPeriods = 8;

        [Column("pvo")]
        public decimal? Pvo { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }

        [Column("hist")]
        public decimal? Histogram { get; set; }
    }
}
