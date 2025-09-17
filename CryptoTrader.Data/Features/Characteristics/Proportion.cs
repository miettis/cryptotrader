using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Data.Features.Characteristics
{
    [Owned]
    public class Proportion
    {
        public decimal Upper { get; set; }
        public decimal Body { get; set; }
        public decimal Lower { get; set; }
    }
}
