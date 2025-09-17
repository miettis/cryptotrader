using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class Pivot
    {
        public static readonly int DefaultLeftSpan = 2;
        public static readonly int DefaultRightSpan = 2;
        public static readonly int DefaultMaxTrendPeriods = 24;
        public static readonly EndType DefaultEndType = EndType.HighLow;

        [Comment("ignore_prediction")]
        public decimal? HighPoint { get; set; }

        [Comment("ignore_prediction")]
        public decimal? LowPoint { get; set; }

        [Comment("ignore_prediction")]
        public decimal? HighLine { get; set; }

        [Comment("ignore_prediction")]
        public decimal? LowLine { get; set; }

        [Comment("ignore_prediction")]
        public PivotTrend HighTrend { get; set; }

        [Comment("ignore_prediction")]
        public PivotTrend LowTrend { get; set; }
    }

    public enum PivotTrend
    {
        None = 0,
        HigherHigh,
        LowerHigh,
        HigherLow,
        LowerLow
    }
}
