using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Cycles
{
    [Owned]
    public class HurstCycleChannel
    {
        [Column("fast_upper")]
        public decimal? FastUpperBand { get; set; }

        [Column("slow_upper")]
        public decimal? SlowUpperBand { get; set; }

        [Column("fast_middle")]
        public decimal? FastMiddleBand { get; set; }

        [Column("slow_middle")]
        public decimal? SlowMiddleBand { get; set; }

        [Column("fast_lower")]
        public decimal? FastLowerBand { get; set; }

        [Column("slow_lower")]
        public decimal? SlowLowerBand { get; set;}

        [Column("omed")]
        public decimal? OMed { get; set; }

        [Column("oshort")]
        public decimal? OShort { get; set; }
    }
}
