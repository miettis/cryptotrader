using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Characteristics
{
    [Owned]
    public class Length
    {
        public decimal Candle { get; set; }
        public decimal Upper { get; set; }
        public decimal Body { get; set; }
        public decimal Lower { get; set; }
    }
}
