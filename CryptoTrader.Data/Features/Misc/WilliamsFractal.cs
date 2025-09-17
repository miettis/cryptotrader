using Microsoft.EntityFrameworkCore;
using Skender.Stock.Indicators;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class WilliamsFractal
    {
        public static readonly int DefaultWindowSpan = 2;
        public static readonly EndType DefaultEndType = EndType.HighLow;


        [Column("bear")]
        [Comment("ignore_prediction")]
        public decimal? FractalBear { get; set; }

        [Column("bull")]
        [Comment("ignore_prediction")]
        public decimal? FractalBull { get; set; }
    }
}
