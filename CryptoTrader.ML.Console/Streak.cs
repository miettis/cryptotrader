namespace CryptoTrader.ML.Console
{
    class Streak
    {
        public int CryptoId { get; set; }
        public int Hours { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}
