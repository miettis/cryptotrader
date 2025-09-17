namespace CryptoTrader.Data
{
    public class CryptoModel
    {
        public int Id { get; set; }

        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string Output { get; set; }
        public string ModelName { get; set; }
        public decimal Accuracy { get; set; }
        public int Samples { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
