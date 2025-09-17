namespace CryptoTrader.Data.Analyzers
{
    public class Analyzer
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public List<AnalyzerOutput> Outputs { get; set; }
    }
    public class AnalyzerOutput
    {
        public int Id { get; set; }
        public int AnalyzerId { get; set; }
        public Analyzer Analyzer { get; set; }
        public string Key { get; set; }
        public bool IsClass { get; set; }
    }
}
