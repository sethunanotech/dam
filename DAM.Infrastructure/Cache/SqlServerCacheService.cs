using DAM.Application.Contracts;
using DAM.Domain.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DAM.Infrastructure.Cache
{
    public class SqlServerCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions _cacheConfigOptions;

        public SqlServerCacheService(IDistributedCache distributedCache, IOptions<SqlServerCacheConfiguration> cacheConfigOptions) { 
            _distributedCache = distributedCache;
            if (cacheConfigOptions != null )
            {
                _cacheConfigOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(cacheConfigOptions.Value.AbsoluteExpirationInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(cacheConfigOptions.Value.SlidingExpirationInHours)
                };
            }
        }
        public bool TryGet<T>(string cacheKey, out T value)
        {
            var bytes = _distributedCache.Get(cacheKey);
            if (bytes is null)
            {
                value = default(T);
                return false;
            }
            value = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
            return true;
        }
        public T Set<T>(string cacheKey, T value)
        {
            var cacheData = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value);
            _distributedCache.Set(cacheKey, cacheData, _cacheConfigOptions);
            return value;
        }
        public void Remove(string cacheKey)
        {
            _distributedCache?.Remove(cacheKey);
        }

        public void RefreshCache<T>(string cacheKey, T Value)
        {
            _distributedCache.Remove(cacheKey);
            var cacheData = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(Value);
            _distributedCache.Set(cacheKey, cacheData, _cacheConfigOptions);
        }
    }
}
