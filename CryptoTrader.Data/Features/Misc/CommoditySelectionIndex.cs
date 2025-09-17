using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class CommoditySelectionIndex
    {
        [Column("csi")]
        public decimal? Csi { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}
