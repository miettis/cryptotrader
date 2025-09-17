using CryptoTrader.Data.Analyzers;

namespace CryptoTrader.Data
{
    public class PriceFeature
    {
        public long Id { get; set; }
        public long PriceId { get; set; }
        public Price Price { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
        public double Value { get; set; }
    }
}
