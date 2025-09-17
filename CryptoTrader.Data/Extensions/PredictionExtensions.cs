namespace CryptoTrader.Data.Extensions
{
    public static class PredictionExtensions
    {
        public static bool GoodTimeToBuy(this PricePrediction prediction)
        {
            var sum = (prediction.Day.Rank2 ?? 2 * 2) + 
                      (prediction.Day.Rank3 ?? 3 * 3) +
                      (prediction.Day.Rank4 ?? 4 * 4) +
                      (prediction.Day.Rank6 ?? 6 * 6) +
                      (prediction.Day.Rank8 ?? 8 * 8) +
                      (prediction.Day.Rank12 ?? 12 * 12);

            var count = 0;
            count += (prediction.Day.Rank2 ?? 2) <= 1 ? 1 : 0;
            count += (prediction.Day.Rank3 ?? 3) <= 2 ? 1 : 0;
            count += (prediction.Day.Rank4 ?? 4) <= 2 ? 1 : 0;
            count += (prediction.Day.Rank6 ?? 6) <= 3 ? 1 : 0;
            count += (prediction.Day.Rank8 ?? 8) <= 4 ? 1 : 0;
            count += (prediction.Day.Rank12 ?? 12) <= 6 ? 1 : 0;

            return sum < 154 && count >= 3;
        }
    }
}
