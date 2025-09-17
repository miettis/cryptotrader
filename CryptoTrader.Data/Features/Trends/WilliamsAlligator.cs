using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class WilliamsAlligator
    {
        public static readonly int DefaultJawPeriods = 12;
        public static readonly int DefaultJawOffset = 6;
        public static readonly int DefaultTeethPeriods = 8;
        public static readonly int DefaultTeethOffset = 6;
        public static readonly int DefaultLipsPeriods = 4;
        public static readonly int DefaultLipsOffset = 2;

        [Column("jaw")]
        public decimal? Jaw { get; set; }

        [Column("teeth")]
        public decimal? Teeth { get; set; }

        [Column("lips")]
        public decimal? Lips { get; set; }
    }
}
