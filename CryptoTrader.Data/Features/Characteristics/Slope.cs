using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Characteristics
{
    [Owned]
    public class SlopeHour
    {
        public decimal? High6 { get; set; }
        public decimal? High8 { get; set; }
        public decimal? High12 { get; set; }
        public decimal? High24 { get; set; }
        public decimal? Low6 { get; set; }
        public decimal? Low8 { get; set; }
        public decimal? Low12 { get; set; }
        public decimal? Low24 { get; set; }
    }

    [Owned]
    public class SlopeMinute
    {
        public decimal? M15 { get; set; }
        public decimal? M30 { get; set; }
        public decimal? M60 { get; set; }
    }
}
