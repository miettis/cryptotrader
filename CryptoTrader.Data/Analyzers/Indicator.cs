namespace CryptoTrader.Data.Analyzers
{
    public class Indicator
    {
        public int Id { get; set; }
        public int AnalyzerId { get; set; }
        public Analyzer Analyzer { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public List<Feature> Features { get; set; }
    }
    public class Feature
    {
        public int Id { get; set; }
        public int IndicatorId { get; set; }
        public Indicator Indicator { get; set; }
        public int OutputId { get; set; }
        public AnalyzerOutput Output { get; set; }
    }
}