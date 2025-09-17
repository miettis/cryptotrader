using CryptoTrader.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CryptoTrader.ML.Console
{
    internal class Analyzer
    {
        public static async Task CalculateStreaks(BinanceContext context)
        {
            var cryptos = await context.Cryptos.OrderBy(x => x.Rank).ToListAsync();
            foreach (var crypto in cryptos)
            {
                var timestamps = await context.Prices.Where(x => x.CryptoId == crypto.Id).OrderBy(x => x.TimeOpen).Select(x => x.TimeOpen).ToListAsync();
                if (timestamps.Count == 0)
                {
                    continue;
                }
                var streak = new Streak
                {
                    CryptoId = crypto.Id,
                    Hours = 0,
                    Start = timestamps[0],
                    End = timestamps[0]
                };
                var streaks = new List<Streak>();
                for (var i = 1; i < timestamps.Count; i++)
                {
                    var timestamp = timestamps[i];
                    if (timestamp == streak.End.AddHours(1))
                    {
                        streak.End = timestamp;
                        continue;
                    }
                    if (timestamp > streak.End.AddHours(1))
                    {
                        streak.Hours = (int)(streak.End - streak.Start).TotalHours + 1;
                        streaks.Add(streak);
                        streak = new Streak
                        {
                            CryptoId = crypto.Id,
                            Hours = 0,
                            Start = timestamp,
                            End = timestamp
                        };
                    }
                }
                streak.Hours = (int)(streak.End - streak.Start).TotalHours + 1;
                streaks.Add(streak);

                File.WriteAllText($"{crypto.Id}_streaks.json", JsonSerializer.Serialize(streaks, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        public static async Task<IEnumerable<Streak>> GetStreaks()
        {
            var streaks = new List<Streak>();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_streaks.json");
            foreach (var file in files)
            {
                var json = await File.ReadAllTextAsync(file);
                var cryptoStreaks = JsonSerializer.Deserialize<List<Streak>>(json);
                streaks.AddRange(cryptoStreaks);
            }
            return streaks;
        }
    }
}
