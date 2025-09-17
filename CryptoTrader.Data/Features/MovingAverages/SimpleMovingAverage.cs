using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.MovingAverages
{
    [Owned]
    public class SimpleMovingAverage
    {
        /// <summary>
        /// Simple moving average
        /// </summary>
        [Column("sma")]
        public decimal? Sma { get; set; }

        /// <summary>
        /// Mean absolute deviation
        /// </summary>
        [Column("mad")]
        public decimal? Mad { get; set; }

        /// <summary>
        /// Mean square error
        /// </summary>
        [Column("mse")]
        public decimal? Mse { get; set; }

        /// <summary>
        /// Mean absolute percentage error
        /// </summary>
        [Column("mape")]
        public decimal? Mape { get; set; }
    }
}
