using Microsoft.ML.Data;

namespace CryptoTrader.ML.Console
{
    internal class ModelStats
    {
        public string Trainer { get; set; }
        public RegressionMetrics Metrics { get; internal set; }
        public float MaxDiff { get; internal set; }
        public float AverageDiff { get; internal set; }
    }
}
