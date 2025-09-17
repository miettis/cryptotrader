using ClosedXML.Excel;
using CryptoTrader.Data;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CryptoTrader.Tax
{
    public class BinanceCsvUtility
    {
        private readonly BinanceContext _context;
        private readonly CultureInfo _culture = new CultureInfo("fi-FI");
        private readonly string _excelTemplate = @"C:\Users\mkode\Downloads\binance\verohallinto_-fifo-laskuri_versio-1.1c (1).xlsm";
        public BinanceCsvUtility(BinanceContext context)
        {
            _context = context;
        }

        public void Export()
        {
            var symbolCell = "H9";
            var startRow = 12;

            var rows = new List<Row>();
            var files = Directory.GetFiles(@"C:\Users\mkode\Downloads\binance", "*.csv", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file).Skip(1);
                foreach(var line in lines)
                {
                    var parts = line.Split(',').Select(x => x.Trim('"')).ToArray();
                    if (parts[2] != "Spot")
                    {
                        continue;
                    }
                    var operation = parts[3] switch
                    {
                        "Buy" => RowType.Receive,
                        "Transaction Buy" => RowType.Receive,
                        "Transaction Revenue" => RowType.Receive,
                        "Sell" => RowType.Give,
                        "Transaction Spend" => RowType.Give,
                        "Transaction Sold" => RowType.Give,
                        "Fee" => RowType.Fee,
                        "Transaction Fee" => RowType.Fee,
                        _ => RowType.Unknown
                    };

                    if(operation == RowType.Unknown)
                    {
                        //Console.WriteLine(line);
                        continue;
                    }

                    var row = new Row
                    {
                        Time = DateTime.Parse(parts[1]),
                        Operation = operation,
                        Coin = parts[4],
                        Change = decimal.Parse(parts[5], CultureInfo.InvariantCulture)
                    };
                    rows.Add(row);
                }
            }

            var feeTotalEur = 0m;

            var workbooks = new Dictionary<string, IXLWorkbook>();

            foreach(var tran in rows.GroupBy(x => x.Time).OrderBy(x => x.Key))
            {
                var symbol = tran.First(x => x.Coin != "USDT").Coin;
                if(symbol == "FXS" || symbol == "DOT" || symbol == "WIN" || symbol=="BTT" || symbol=="DOGE")
                {
                    // ei myyty
                    continue;
                }
                workbooks.TryGetValue(symbol, out var excel);
                if(excel == null)
                {
                    var file = $"{symbol}.xlsm";
                    File.Copy(_excelTemplate, file, true);
                    excel = new ClosedXML.Excel.XLWorkbook(file);
                    excel.Worksheet("FIFO-laskuri").Cell(symbolCell).Value = symbol;
                    workbooks.Add(symbol, excel);
                }

                var sheet = excel.Worksheet("FIFO-laskuri");

                var hour = new DateTimeOffset(tran.Key.Year, tran.Key.Month, tran.Key.Day, tran.Key.Hour, 0, 0, TimeSpan.Zero);
                var symbolUsdt = _context.Prices.FirstOrDefault(x => x.Crypto.Symbol == symbol + "USDT" && x.TimeOpen == hour);
                var eurUsdt = _context.Prices.FirstOrDefault(x => x.Crypto.Symbol == "EURUSDT" && x.TimeOpen == hour);


                var receiveSymbol = tran.First(x => x.Operation == RowType.Receive).Coin;
                var giveSymbol = tran.First(x => x.Operation == RowType.Give).Coin;
                var receiveTotal = Math.Abs(tran.Where(x => x.Operation == RowType.Receive).Sum(x => x.Change));
                var giveTotal = Math.Abs(tran.Where(x => x.Operation == RowType.Give).Sum(x => x.Change));
                var feeTotal = Math.Abs(tran.Where(x => x.Operation == RowType.Fee).Sum(x => x.Change));

                var time = tran.Key.ToString("yyyy'-'MM'-'dd HH':'mm");
                

                var row = startRow;
                while (true)
                {
                    if (sheet.Cell($"A{row}").Value.IsBlank)
                    {
                        break;
                    }
                    row++;
                }

                sheet.Cell($"A{row}").SetValue(time);
                sheet.Cell($"F{row}").SetValue("Binance");

                // SELL
                if (receiveSymbol == "USDT")
                {
                    var amount = giveTotal;
                    var totalUsdt = amount * symbolUsdt.Open;
                    var totalEur = totalUsdt / eurUsdt.Open;
                    var price = totalEur / amount;
                    var feeEur = feeTotal / eurUsdt.Open;
                    feeTotalEur += feeEur;

                    
                    sheet.Cell($"B{row}").SetValue("Myynti");
                    sheet.Cell($"C{row}").SetValue(amount);
                    sheet.Cell($"D{row}").SetValue(price);
                    sheet.Cell($"E{row}").SetValue(totalEur);
                    


                    var line = $"{time}\tMyynti\t{amount.ToString(_culture)}\t{price.ToString(_culture)}\t{totalEur.ToString(_culture)}\tBinance";
                    Console.WriteLine(line);
                    //File.AppendAllLines(file, [line]);


                }
                // BUY
                else
                {
                    var amount = receiveTotal - feeTotal;
                    var totalUsdt = giveTotal;
                    var totalEur = totalUsdt / eurUsdt.Open;
                    var price = totalEur / amount;

                    sheet.Cell($"B{row}").SetValue("Osto");
                    sheet.Cell($"C{row}").SetValue(amount);
                    sheet.Cell($"D{row}").SetValue(price);
                    sheet.Cell($"E{row}").SetValue(totalEur);

                    var line = $"{time}\tOsto\t{amount.ToString(_culture)}\t{price.ToString(_culture)}\t{totalEur.ToString(_culture)}tBinance";
                    Console.WriteLine(line);

                    //File.AppendAllLines(file, [line]);
                }



            }

            Console.WriteLine("Fees total: "+feeTotalEur.Round(2));

            foreach(var excel in workbooks.Values)
            {
                excel.Save();
            }
        }
        public enum RowType
        {
            Unknown,
            Give,
            Receive,
            Fee
        }
        public class Row
        {
            public DateTime Time { get; set; }
            public RowType Operation { get; set; }
            public string Coin { get; set; }
            public decimal Change { get; set; }
        }
    }
}
