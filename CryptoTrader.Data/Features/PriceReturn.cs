using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceReturn : FeatureContainer
    {
        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [Column("day")]
        public ReturnValue Day { get; set; } = new ReturnValue();

        [Column("twoday")]
        public ReturnValue TwoDay { get; set; } = new ReturnValue();

        [Column("week")]
        public ReturnValue Week { get; set; } = new ReturnValue();

    }
    [Owned]
    public class ReturnValue
    {
        [Column("return")]
        public decimal? Return { get; set; }

        [Column("interval")]
        public int? Interval { get; set; }

        [Column("rank2")]
        public int? Rank2 { get; set; }

        [Column("rank3")]
        public int? Rank3 { get; set; }

        [Column("rank4")]
        public int? Rank4 { get; set; }

        [Column("rank6")]
        public int? Rank6 { get; set; }

        [Column("rank8")]
        public int? Rank8 { get; set; }

        [Column("rank12")]
        public int? Rank12 { get; set; }

    }
}
