using CryptoTrader.Data.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO.Compression;
using System.Net;

namespace CryptoTrader.Data
{
    public class BinanceImport
    {
        private readonly HttpClient _httpClient;
        private readonly BinanceImportConfig _config;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger _logger;
        public BinanceImport(IDbContextFactory<BinanceContext> contextFactory, ILogger logger, BinanceImportConfig config)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _httpClient = new HttpClient();
            _config = config;
        }

        public async Task FetchMonthly(string symbol, string interval, DateOnly start, DateOnly end)
        {
            var date = new DateOnly(start.Year, start.Month, 1);
            while (date <= end)
            {
                try
                {
                    await FetchMonthly(symbol, interval, date.Year, date.Month);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogInformation($"Missing {symbol} {date.Month:D2}/{date.Year}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error {symbol} {date.Month:D2}/{date.Year} | {ex.Message}");
                }
                date = date.AddMonths(1);
            }
        }

        public async Task FetchMonthly(string symbol, string interval, int year, int month)
        {
            var directory = Path.Combine(_config.MonthlyDirectory, interval, symbol);
            var outputFile = Path.Combine(directory, $@"{symbol}-{interval}-{year}-{month:D2}.csv");
            if (File.Exists(outputFile))
            {
                _logger.LogInformation($"Monthly file found {symbol} {month:D2}/{year}");
                return;
            }
            var url = $"https://data.binance.vision/data/spot/monthly/klines/{symbol}/{interval}/{symbol}-{interval}-{year}-{month:D2}.zip";
            var data = await _httpClient.GetByteArrayAsync(url);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            ExtractZipToDirectory(data, directory);
            _logger.LogInformation($"Monthly file fetched {symbol} {month:D2}/{year}");

            if (_config.SleepAfterFetch > 0)
            {
                Thread.Sleep(_config.SleepAfterFetch);
            }
        }

        public async Task FetchDaily(string symbol, string interval, DateOnly start, DateOnly end)
        {
            var date = start;
            while (date <= end)
            {
                try
                {
                    await FetchDaily(symbol, interval, date);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogInformation($"Missing {symbol} {date}");
                        date = date.AddDays(30);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error {symbol} {date} | {ex.Message}");
                }
                date = date.AddDays(1);
            }
        }
        public async Task FetchDaily(string symbol, string interval, DateOnly date)
        {
            var directory = Path.Combine(_config.DailyDirectory, interval, symbol);
            var outputFile = Path.Combine(directory, $@"{symbol}-{interval}-{date.Year}-{date.Month:D2}-{date.Day:D2}.csv");
            if (File.Exists(outputFile))
            {
                _logger.LogInformation($"Daily file found {symbol} {date}");
                return;
            }
            var url = $"https://data.binance.vision/data/spot/daily/klines/{symbol}/{interval}/{symbol}-{interval}-{date.Year}-{date.Month:D2}-{date.Day:D2}.zip";
            var data = await _httpClient.GetByteArrayAsync(url);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            ExtractZipToDirectory(data, directory);
            _logger.LogInformation($"Daily file fetched {symbol} {date}");

            if (_config.SleepAfterFetch > 0)
            {
                Thread.Sleep(_config.SleepAfterFetch);
            }
        }

        public async Task<DateTimeOffset> ImportMonthly(string interval, DateOnly start, DateOnly end, string[]? symbols = null)
        {
            var dir = Path.Combine(_config.MonthlyDirectory, interval);
            var maxTimestamp = DateTimeOffset.MinValue;
            foreach (var file in Directory.GetFiles(dir, "*.csv", SearchOption.AllDirectories))
            {
                var filenameParts = Path.GetFileName(file).Split('-', '.');
                var symbol = filenameParts[0];
                if (symbols != null && !symbols.Contains(symbol))
                {
                    continue;
                }

                var year = int.Parse(filenameParts[2]);
                var month = int.Parse(filenameParts[3]);
                var fileStart = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
                var fileEnd = fileStart.AddMonths(1).AddMicroseconds(-1);
                if (fileEnd.Date < new DateTime(start, TimeOnly.MinValue) || fileStart > new DateTime(end, TimeOnly.MaxValue))
                {
                    continue;
                }

                var context = GetContext();
                var crypto = context.Cryptos.FirstOrDefault(x => x.Symbol == symbol);
                if (crypto == null)
                {
                    continue;
                }

                _logger.LogInformation($"Processing {file}");

                HashSet<DateTimeOffset> timestamps = new HashSet<DateTimeOffset>();
                if (interval == "1m")
                {
                    /*
                    timestamps = context.PriceMinute.Where(x => x.CryptoId == crypto.Id && x.TimeOpen >= fileStart && x.TimeOpen <= fileEnd)
                        .Select(x => x.TimeOpen)
                        .ToHashSet();
                    */
                }
                else if (interval == "1h")
                {
                    timestamps = context.Prices.Where(x => x.CryptoId == crypto.Id && x.TimeOpen >= fileStart && x.TimeOpen <= fileEnd)
                        .Select(x => x.TimeOpen)
                        .ToHashSet();
                }

                foreach (var line in File.ReadLines(file))
                {
                    if (interval == "1m")
                    {
                        /*
                        var price = CreatePrice<PriceMinute>(line);
                        if (timestamps.Contains(price.TimeOpen))
                        {
                            continue;
                        }

                        price.CryptoId = crypto.Id;
                        context.PriceMinute.Add(price);

                        if(price.TimeOpen > maxTimestamp)
                        {
                            maxTimestamp = price.TimeOpen;
                        }
                        */
                    }
                    else if (interval == "1h")
                    {
                        var price = CreatePrice<Price>(line);
                        if (timestamps.Contains(price.TimeOpen))
                        {
                            continue;
                        }
                        price.CryptoId = crypto.Id;
                        context.Prices.Add(price);

                        if (price.TimeOpen > maxTimestamp)
                        {
                            maxTimestamp = price.TimeOpen;
                        }
                    }
                }

                try
                {
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error saving changes {file} | {ex.Message}");
                }
                
                   
                if (_config.DeleteFileAfterInsert)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        _logger.LogError($"Error deleting {file}");
                    }
                }
            }

            return maxTimestamp;
        }
        public async Task<DateTimeOffset> ImportDaily(string interval, DateOnly start, DateOnly end, string[]? symbols = null)
        {
            var dir = Path.Combine(_config.DailyDirectory, interval);
            var maxTimestamp = DateTimeOffset.MinValue;
            foreach (var file in Directory.GetFiles(dir, "*.csv", SearchOption.AllDirectories))
            {
                var filenameParts = Path.GetFileName(file).Split('-', '.');
                var symbol = filenameParts[0];
                if (symbols != null && !symbols.Contains(symbol))
                {
                    continue;
                }

                var year = int.Parse(filenameParts[2]);
                var month = int.Parse(filenameParts[3]);
                var day = int.Parse(filenameParts[4]);
                var fileStart = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
                var fileEnd = fileStart.AddDays(1).AddMicroseconds(-1);
                if (fileEnd.Date < new DateTime(start, TimeOnly.MinValue) || fileStart > new DateTime(end, TimeOnly.MaxValue))
                {
                    continue;
                }

                var context = GetContext();
                var crypto = context.Cryptos.FirstOrDefault(x => x.Symbol == symbol);
                if (crypto == null)
                {
                    continue;
                }

                _logger.LogInformation($"Processing {file}");

                HashSet<DateTimeOffset> timestamps = new HashSet<DateTimeOffset>();
                if (interval == "1m")
                {
                    /*
                    timestamps = context.PriceMinute.Where(x => x.CryptoId == crypto.Id && x.TimeOpen >= fileStart && x.TimeOpen <= fileEnd)
                        .Select(x => x.TimeOpen)
                        .ToHashSet();
                    */
                }
                else if (interval == "1h")
                {
                    timestamps = context.Prices.Where(x => x.CryptoId == crypto.Id && x.TimeOpen >= fileStart && x.TimeOpen <= fileEnd)
                        .Select(x => x.TimeOpen)
                        .ToHashSet();
                }

                foreach (var line in File.ReadLines(file))
                {
                    if (interval == "1m")
                    {
                        /*
                        var price = CreatePrice<PriceMinute>(line);
                        if (timestamps.Contains(price.TimeOpen))
                        {
                            continue;
                        }

                        price.CryptoId = crypto.Id;
                        context.PriceMinute.Add(price);

                        if (price.TimeOpen > maxTimestamp)
                        {
                            maxTimestamp = price.TimeOpen;
                        }
                        */
                    }
                    else if (interval == "1h")
                    {
                        var price = CreatePrice<Price>(line);
                        if (timestamps.Contains(price.TimeOpen))
                        {
                            continue;
                        }
                        price.CryptoId = crypto.Id;
                        context.Prices.Add(price);

                        if (price.TimeOpen > maxTimestamp)
                        {
                            maxTimestamp = price.TimeOpen;
                        }
                    }
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving changes {file} | {ex.Message}");
                }

                if (_config.DeleteFileAfterInsert)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        _logger.LogError($"Error deleting {file}");
                    }
                }
            }
            return maxTimestamp;
        }

        public void UpdateHourMovingAverages(DateOnly start, DateOnly end)
        {
            var hoursInWeek = 7 * 24;

            var priceStart = new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero);
            var dataStart = priceStart.AddHours(-hoursInWeek);
            var dataEnd = new DateTimeOffset(end, TimeOnly.MaxValue, TimeSpan.Zero);

            var context = GetContext();
            var cryptos = context.Cryptos.OrderByDescending(x => x.Rank).ToList();

            foreach (var crypto in cryptos)
            {
                var context2 = GetContext();
                var prices = context2.Prices.Where(x => x.CryptoId == crypto.Id && x.TimeOpen >= dataStart && x.TimeOpen <= dataEnd).OrderBy(x => x.TimeOpen).ToArray();
                FeatureCalculation.UpdateMovingAverages(prices, priceStart);

                context2.SaveChanges();
            }
        }
        /*
        public void UpdateMinuteMovingAverages(DateOnly start, DateOnly end)
        {
            var minutesInHour = 60;

            var priceStart = new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero);
            var dataStart = priceStart.AddMinutes(-minutesInHour);
            var dataEnd = new DateTimeOffset(end, TimeOnly.MaxValue, TimeSpan.Zero);

            var context = GetContext();
            var cryptos = context.Cryptos.OrderByDescending(x => x.Rank).ToList();

            foreach (var crypto in cryptos)
            {
                var context2 = GetContext();
                var prices = context2.PriceMinute.Where(x => x.CryptoId == crypto.Id && x.TimeOpen >= dataStart && x.TimeOpen <= dataEnd).OrderBy(x => x.TimeOpen).ToArray();
                FeatureCalculation.UpdateMovingAverages(prices, priceStart);

                context2.SaveChanges();
            }
        }
        */
        private T CreatePrice<T>(string line) where T : Price, new()
        {
            var culture = CultureInfo.InvariantCulture;
            var parts = line.Split(',');
            var price = new T();
            price.Timestamp = long.Parse(parts[0]);
            // 1734476400000
            // 1740700800000000
            if (parts[0].Length == 13)
            {
                price.TimeOpen = DateTimeOffset.FromUnixTimeMilliseconds(price.Timestamp);
                ;
            }
            else if (parts[0].Length == 16)
            {
                price.TimeOpen = DateTimeOffset.FromUnixTimeMilliseconds(price.Timestamp / 1000);
                ;
            }
            else
            {
                throw new Exception("Invalid timestamp length");
            }
            price.Open = decimal.Parse(parts[1], culture);
            price.High = decimal.Parse(parts[2], culture);
            price.Low = decimal.Parse(parts[3], culture);
            price.Close = decimal.Parse(parts[4], culture);
            price.Volume = decimal.Parse(parts[5], culture);
            // close time
            price.QuoteVolume = decimal.Parse(parts[7], culture);
            price.Trades = long.Parse(parts[8]);
            price.BuyVolume = decimal.Parse(parts[9], culture);
            price.BuyQuoteVolume = decimal.Parse(parts[10], culture);

            price.PopulateCalculatedValues();

            return price;
        }

        private void ExtractZipToDirectory(byte[] data, string directory)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                using (var archive = new ZipArchive(memoryStream))
                {
                    archive.ExtractToDirectory(directory, true);
                }
            }
        }

        private BinanceContext GetContext()
        {
            return _contextFactory.CreateDbContext();
        }

        private IEnumerable<(DateTimeOffset StartTime, int Count)> GetStreaksHourly(int cryptoId)
        {
            var context = GetContext();
            var timestamps = context.Prices.Where(x => x.CryptoId == cryptoId).OrderBy(x => x.TimeOpen).Select(x => x.TimeOpen).ToList();
            var streaks = new List<(DateTimeOffset, int)>();
            var streak = (timestamps[0], 1);
            for (var i = 1; i < timestamps.Count; i++)
            {
                if (timestamps[i] != timestamps[i - 1].AddHours(1))
                {
                    streaks.Add(streak);
                    streak = (timestamps[i], 1);
                }
                else
                {
                    streak.Item2++;
                }
            }
            streaks.Add(streak);

            return streaks;
        }
    }
    public class BinanceImportConfig
    {
        public string MonthlyDirectory { get; set; }
        public string DailyDirectory { get; set; }
        public bool DeleteFileAfterInsert { get; set; }
        public bool UpdateDataStartTime { get; set; }
        public int SleepAfterFetch { get; set; }
    }
}
