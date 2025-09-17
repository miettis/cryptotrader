using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;
namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class MovingAverageEnvelope
    {
        public static readonly int DefaultLoopbackPeriods = 6;
        public static readonly double DefaultPercentOffset = 2.5;
        public static readonly MaType DefaultType = MaType.SMA;

        [Column("upper")]
        public decimal? Upper { get; set; }

        [Column("center")]
        public decimal? Center { get; set; }

        [Column("lower")]
        public decimal? Lower { get; set; }
    }
}
