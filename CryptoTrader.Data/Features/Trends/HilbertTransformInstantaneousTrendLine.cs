using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class HilbertTransformInstantaneousTrendLine
    {
        [Column("dcp")]
        public int? DominantCyclePeriods { get; set; }

        [Column("trend")]
        public decimal? TrendLine { get; set; }

        [Column("smooth")]
        public decimal? SmoothPrice { get; set; }
    }
}
