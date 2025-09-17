using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersSpectrumDerivedFilterBankAnalyzer : StockDataAnalyzerBase<EhlersSpectrumDerivedFilterBankAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersSpectrumDerivedFilterBank(settings.MinLength, settings.MaxLength, settings.Length1, settings.Length2);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Esdfb", results.OutputValues["Esdfb"] }
            };
        }

        public class Settings
        {
            public int MinLength { get; set; } = 8;
            public int MaxLength { get; set; } = 50;
            public int Length1 { get; set; } = 40;
            public int Length2 { get; set; } = 10;
        }
    }
}
