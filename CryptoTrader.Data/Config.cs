using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Data
{
    public class Config
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
