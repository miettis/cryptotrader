using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Data.Features.Volatility
{
    [Owned]
    public class BollingerBand
    {
        public static readonly int DefaultLoopbackPeriods = 24;
        public static readonly int DefaultStandardDeviation = 2;


        // SMA already in another indicator
        //public decimal? SMA { get; set; }

        /// <summary>
        /// Upper line is D standard deviations above the SMA
        /// </summary>
        //[Column("upper")]
        public decimal? Upper { get; set; }

        /// <summary>
        /// Lower line is D standard deviations below the SMA
        /// </summary>
        //[Column("lower")]
        public decimal? Lower { get; set; }

        /// <summary>
        /// %B is the location within the bands. (Price-LowerBand)/(UpperBand-LowerBand)
        /// </summary>
        //[Column("perb")]
        public decimal? PercentB { get; set; }

        /// <summary>
        /// Z-Score of current price (number of standard deviations from mean)
        /// </summary>
        //[Column("z")]
        public decimal? ZScore { get; set; }

        /// <summary>
        /// Width as percent of SMA price. (UpperBand-LowerBand)/Sma
        /// </summary>
        //[Column("width")]
        public decimal? Width { get; set; }
    }
}
