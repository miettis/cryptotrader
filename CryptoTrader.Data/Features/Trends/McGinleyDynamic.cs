using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class McGinleyDynamic
    {

        [Column("h6")]
        public decimal? McGinleyDynamic6 { get; set; }

        [Column("h12")]
        public decimal? McGinleyDynamic12 { get; set; }

        [Column("h24")]
        public decimal? McGinleyDynamic24 { get; set; }

        [Column("h168")]
        public decimal? McGinleyDynamic168 { get; set; }
    }
}
