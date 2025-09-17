using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class StochasticOscillator
    {
        public static readonly int DefaultLookbackPeriods = 12;
        public static readonly int DefaultSignalPeriods = 3;
        public static readonly int DefaultSmoothPeriods = 3;
        public static readonly int DefaultKFactor = 3;
        public static readonly int DefaultDFactor = 2;
        public static readonly MaType DefaultMovingAverageType = MaType.SMA;

        /// <summary>
        /// %K Oscillator
        /// </summary>
        [Column("k")]
        public decimal? Oscillator { get; set; }

        /// <summary>
        /// %D Simple moving average of Oscillator
        /// </summary>
        [Column("d")]
        public decimal? Signal { get; set; }

        /// <summary>
        /// %J is the weighted divergence of %K and %D: %J = kFactor × %K - dFactor × %D
        /// </summary>
        [Column("j")]
        public decimal? PercentJ { get; set; }
    }
}
