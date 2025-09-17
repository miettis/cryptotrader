using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class PivotPoints
    {
        public static readonly PeriodSize DefaultWindowSize = PeriodSize.Day;
        public static readonly PivotPointType DefaultPivotPointType = PivotPointType.Standard;

        public static readonly int DefaultRollingWindowPeriods = 12;
        public static readonly int DefaultRollingOffsetPeriods = 3;
        public static readonly PivotPointType DefaultRollingPivotPointType = PivotPointType.Standard;

        /// <summary>
        /// Resistance level 1
        /// </summary>
        [Column("r1")]
        public decimal? R1 { get; set; }

        /// <summary>
        /// Resistance level 2
        /// </summary>
        [Column("r2")]
        public decimal? R2 { get; set; }

        /// <summary>
        /// Resistance level 1
        /// </summary>
        [Column("r3")]
        public decimal? R3 { get; set; }

        /// <summary>
        /// Pivot point
        /// </summary>
        [Column("pp")]
        public decimal? PP { get; set; }

        /// <summary>
        /// Support level 1
        /// </summary>
        [Column("s1")]
        public decimal? S1 { get; set; }

        /// <summary>
        /// Support level 2
        /// </summary>
        [Column("s2")]
        public decimal? S2 { get; set; }

        /// <summary>
        /// Support level 3
        /// </summary>
        [Column("s3")]
        public decimal? S3 { get; set; }

    }
}
