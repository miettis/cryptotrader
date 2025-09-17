using Microsoft.ML.Data;

namespace CryptoTrader.ML.Console
{
    public class PricePrediction
    {
        [ColumnName("Score")]
        public float Output;
    }
}
