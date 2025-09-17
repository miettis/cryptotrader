using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PricePeak : FeatureContainer
    {
        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [Column("highest_high")]
        [Comment("future")]
        public bool HighestHigh { get; set; }

        [Column("offset_prev_hh")]
        public int? OffsetPreviousHH { get; set; }

        [Column("offset_next_hh")]
        public int? OffsetNextHH { get; set; }

        [Column("lowest_low")]
        public bool LowestLow { get; set; }

        [Column("offset_prev_ll")]
        public int? OffsetPreviousLL { get; set; }

        [Column("offset_next_ll")]
        public int? OffsetNextLL { get; set; }
    }
}
