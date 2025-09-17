using CryptoTrader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Tax
{
    public class BinanceDbUtility
    {
        private readonly BinanceContext _context;
        public BinanceDbUtility(BinanceContext context)
        {
            _context = context;
        }
        public void Export()
        {
            var orders = _context.Orders.Where(x => x.Status == OrderStatus.Filled).OrderBy(x => x.Updated);
            var orderRelations = _context.OrderRelations.ToList();
            var eurUsdt = _context.Prices.Where(x => x.Crypto.Symbol == "EURUSDT").OrderBy(x => x.TimeOpen).ToList();

            var allFile = $"all.csv";
            if (File.Exists(allFile))
            {
                File.Delete(allFile);
            }
            var header = "Aika;Tapahtuma;Krypto;Määrä;Hinta USDT;Yht USDT;Kulut USDT;Kurssi;Hinta EUR;YHT EUR;Kulut EUR;Määrä jäljellä;Voitto/Tappio;";
            File.AppendAllLines(allFile, [header], Encoding.UTF8);

            foreach (var group in orders.GroupBy(x => x.Symbol))
            {
                var fifoFile = $"{group.Key}_fifo.csv";
                var fullFile = $"{group.Key}.csv";
                if (File.Exists(fifoFile))
                {
                    File.Delete(fifoFile);
                }
                if (File.Exists(fullFile))
                {
                    File.Delete(fullFile);
                }

                var history = new List<OrderHistory>();

                File.AppendAllLines(fullFile, [header], Encoding.UTF8);

                // aika;tapahtuma;määrä;hinta(EUR);yhteensä(EUR);lähde;
                var headerFifo = "time;side;crypto amount;price EUR;total EUR;Market;";
                File.AppendAllLines(fifoFile, [headerFifo]);

                var quantityLeft = 0m;
                foreach (var order in group.OrderBy(x => x.Updated))
                {
                    var time = order.Updated.Value.ToString("s");
                    var side = order.Side == OrderSide.Buy ? "Osto" : "Myynti";
                    var quantity = order.ExecutedQuantity;
                    var priceUSDT = order.AverageFillPrice;

                    var startOfHour = order.Updated.Value.StartOfHour();
                    var latestEurUsdt = eurUsdt.FirstOrDefault(x => x.TimeOpen == startOfHour.AddHours(-1));
                    if (latestEurUsdt == null)
                    {
                        ;
                    }
                    var exchangeRate = latestEurUsdt.Close;

                    var priceEUR = priceUSDT / exchangeRate;

                    if (order.Side == OrderSide.Buy)
                    {
                        var commissionCrypto = 0.001m * quantity;
                        var receivedCrypto = quantity - commissionCrypto;

                        var totalUSDT = quantity * priceUSDT;
                        var commissionUSDT = commissionCrypto * priceUSDT;
                        var receivedUSDT = receivedCrypto * priceUSDT;

                        var receivedEUR = receivedUSDT / exchangeRate;
                        var totalEUR = totalUSDT / exchangeRate;
                        var commissionEUR = Math.Round(commissionUSDT.Value / exchangeRate, 2);

                        quantityLeft += receivedCrypto.Value;

                        // aika;tyyppi;krypto;määrä;hinta(USDT);yhteensä(USDT);kulut(USDT);EUR-USDT;hinta(EUR);yhteensä(EUR);kulut(EUR);jäljellä;voitto/tappio;
                        var line = $"{time};{side};{group.Key};{receivedCrypto};{priceUSDT};{receivedUSDT};{commissionUSDT};{exchangeRate};{priceEUR};{receivedEUR};{commissionEUR};{quantityLeft};;";
                        File.AppendAllLines(fullFile, [line], Encoding.UTF8);
                        File.AppendAllLines(allFile, [line], Encoding.UTF8);

                        // aika;tapahtuma;määrä;hinta(EUR);yhteensä(EUR);lähde;
                        var lineFifo = $"{time};{side};{receivedCrypto};{priceEUR};{receivedEUR};Binance;";
                        File.AppendAllLines(fifoFile, [lineFifo], Encoding.UTF8);

                        history.Add(new OrderHistory(order, receivedCrypto.Value, priceEUR.Value));

                    }
                    if (order.Side == OrderSide.Sell)
                    {
                        var totalUSDT = quantity * priceUSDT;
                        var commissionUSDT = 0.001m * totalUSDT;
                        var netUSDT = totalUSDT;// - commissionUSDT;

                        var totalEUR = totalUSDT / exchangeRate;
                        var commissionEUR = Math.Round(commissionUSDT.Value / exchangeRate, 2);
                        var netEUR = netUSDT / exchangeRate;
                        quantityLeft -= quantity.Value;

                        // aika;tapahtuma;määrä;hinta(EUR);yhteensä(EUR);lähde;
                        var lineFifo = $"{time};{side};{quantity};{priceEUR};{totalEUR};Binance;";
                        File.AppendAllLines(fifoFile, [lineFifo], Encoding.UTF8);

                        var unmatchedQuantity = quantity.Value;
                        var purchaseTotalEUR = 0m;
                        while (unmatchedQuantity > 0)
                        {
                            var buyOrder = history.First(x => x.Unmatched > 0);
                            var quantityToMatch = Math.Min(unmatchedQuantity, buyOrder.Unmatched);
                            unmatchedQuantity -= quantityToMatch;
                            buyOrder.Unmatched -= quantityToMatch;
                            purchaseTotalEUR += quantityToMatch * buyOrder.PriceEUR;
                        }

                        var profitLoss = Math.Round(netEUR.Value - purchaseTotalEUR, 2);

                        // aika;tyyppi;määrä;hinta(USDT);yhteensä(USDT);kulut(USDT);EUR-USDT;hinta(EUR);yhteensä(EUR);kulut(EUR);jäljellä;voitto/tappio;
                        var line = $"{time};{side};{group.Key};{quantity};{priceUSDT};{netUSDT};{commissionUSDT};{exchangeRate};{priceEUR};{netEUR};{commissionEUR};{quantityLeft};{profitLoss}";
                        File.AppendAllLines(fullFile, [line], Encoding.UTF8);
                        File.AppendAllLines(allFile, [line], Encoding.UTF8);

                        history.Add(new OrderHistory(order, 0, priceEUR.Value));
                    }

                }
            }
        }

        class OrderHistory
        {
            public Order Order { get; set; }
            public decimal Unmatched { get; set; }
            public decimal PriceEUR { get; set; }

            public OrderHistory(Order order, decimal unmatched, decimal priceEUR)
            {
                Order = order;
                Unmatched = unmatched;
                PriceEUR = priceEUR;
            }
        }
    }
}
