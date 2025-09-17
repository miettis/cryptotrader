using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class StochasticRSI
    {
        public static readonly int DefaultRsiPeriods = 12;
        public static readonly int DefaultStochPeriods = 12;
        public static readonly int DefaultSignalPeriods = 3;
        public static readonly int DefaultSmoothPeriods = 1;

        /// <summary>
        /// %K Oscillator = Stochastic RSI = Stoch(S,G,M) of RSI(R) of price
        /// </summary>
        [Column("rsi")]
        public decimal? StochRsi { get; set; }

        /// <summary>
        /// %D Signal Line = Simple moving average of %K based on G periods
        /// </summary>
        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}
