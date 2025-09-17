namespace CryptoTrader.Data.Extensions
{
    public static class AssetExtensions
    {
        public static readonly string QuoteAsset = "USDT";

        public static string AsSymbolPair(this string symbol)
        {
            if (!symbol.EndsWith(QuoteAsset))
            {
                return symbol + QuoteAsset;
            }
            return symbol;
        }

        public static string AsBaseAsset(this string symbol)
        {
            if (symbol.EndsWith(QuoteAsset) && symbol != QuoteAsset)
            {
                return symbol.Substring(0, symbol.Length - QuoteAsset.Length);
            }
            return symbol;
        }
    }
}
