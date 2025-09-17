using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class VortexIndicator
    {
        public static readonly int DefaultLookbackPeriods = 18;


        [Column("pvi")]
        public decimal? Pvi { get; set; }

        [Column("nvi")]
        public decimal? Nvi { get; set; }
    }
}
