using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Data.Features.Characteristics
{
    [Owned]
    public class Average
    {
        public decimal OHLC { get; set; }
        public decimal HLC { get; set; }
        public decimal HL { get; set; }
        public decimal OC { get; set; }
    }
}
