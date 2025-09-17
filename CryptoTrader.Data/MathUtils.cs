namespace CryptoTrader.Data
{
    public static class MathUtils
    {
        public static (decimal Average, decimal Std) GetAvgAndStd(decimal[] values)
        {
            var avg = values.Average();
            var std = Math.Sqrt(values
                       .Select(x => Math.Pow((double)x - (double)avg, 2))
                       .Average());

            return (avg, (decimal)std);
        }
        public static (decimal Average, decimal Std) GetWeightedAvgAndStd(decimal[] values)
        {
            var sumOfWeights = values.Select((_, index) => index + 1).Sum();

            var avg = values.Select((x, index) => (index + 1) * x).Sum() / sumOfWeights;

            var std = Math.Sqrt(values
                       .Select((x, index) => (index + 1) * Math.Pow((double)x - (double)avg, 2)).Sum() / sumOfWeights);

            return (avg, (decimal)std);
        }
    }
}
