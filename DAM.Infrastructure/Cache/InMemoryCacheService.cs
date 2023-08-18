using DAM.Application.Contracts;
using DAM.Domain.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DAM.Infrastructure.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly InMemoryCacheConfiguration? _cacheConfig;
        private readonly MemoryCacheEntryOptions? _cacheOptions;

        public InMemoryCacheService(IMemoryCache memoryCache, IOptions<InMemoryCacheConfiguration> cacheConfig) 
        { 
            _memoryCache = memoryCache;
            _cacheConfig = cacheConfig.Value;

            if (_cacheConfig != null )
            {
                _cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes),
                    Priority = CacheItemPriority.High
                };
            }
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            var isCacheAvailable = _memoryCache.TryGetValue(cacheKey, out value);
            if (value == null) return isCacheAvailable;
            else return isCacheAvailable;
        }

        public T Set<T>(string cacheKey, T value)
        {
            return _memoryCache.Set(cacheKey, value, _cacheOptions);
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}
