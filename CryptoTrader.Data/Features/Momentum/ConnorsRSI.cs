using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class ConnorsRSI
    {
        public static readonly int DefaultRsiPeriods = 3;
        public static readonly int DefaultStreakPeriods = 2;
        public static readonly int DefaultRankPeriods = 100;

        [Column("rsi")]
        public decimal? Rsi { get; set; }

        [Column("streak")]
        public decimal? RsiStreak { get; set; }

        [Column("rank")]
        public decimal? PercentRank { get; set; }

        [Column("crsi")]
        public decimal? ConnorsRsi { get; set; }
    }
}
