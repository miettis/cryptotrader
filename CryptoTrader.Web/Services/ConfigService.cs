using CryptoTrader.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CryptoTrader.Web.Services
{
    public class ConfigService
    {
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private Dictionary<string,object> _cache = new Dictionary<string, object>();
        public ConfigService(IDbContextFactory<BinanceContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<T> GetConfig<T>() where T : class
        {
            var type = typeof(T).Name;
            if(_cache.TryGetValue(type, out var result))
            {
                return (T)result;
            }
            var context = await _contextFactory.CreateDbContextAsync();
            var config = await context.Configs.FirstOrDefaultAsync(x => x.Type == type);
            if(config != null && config.Value != null)
            {
                var value = JsonConvert.DeserializeObject<T>(config.Value);
                _cache[type] = value;
                return value;
            }
            return null;
        }

        public async Task SaveConfig<T>(T value) where T : class
        {
            var type = typeof(T).Name;
            var context = await _contextFactory.CreateDbContextAsync();
            var config = await context.Configs.FirstOrDefaultAsync(x => x.Type == type);
            if(config == null)
            {
                config = new Config
                {
                    Type = type,
                    Value = JsonConvert.SerializeObject(value)
                };
                await context.Configs.AddAsync(config);
            }
            else
            {
                config.Value = JsonConvert.SerializeObject(value);
            }

            await context.SaveChangesAsync();
            _cache[type] = value;
        }
    }
}
