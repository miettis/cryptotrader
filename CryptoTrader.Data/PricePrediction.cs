using CryptoTrader.Data.Features;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data
{
    public class PricePrediction : FeatureContainer
    {
        public int Id { get; set; }

        public Crypto Crypto { get; set; }

        public int CryptoId { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset TimeOpen { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        [Column("day")]
        public PredictionRank Day { get; set; } = new PredictionRank();

        [Column("twoday")]
        public PredictionRank TwoDay { get; set; } = new PredictionRank();

        [Column("week")]
        public PredictionRank Week { get; set; } = new PredictionRank();
    }

    [Owned]
    public class PredictionRank
    {
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
