
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Catalogo.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrCreate<T>(string key, Func<Task<T>> createItem)
        {
            if (!_memoryCache.TryGetValue(key,out T? value))
            {
                value = await createItem();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                     AbsoluteExpirationRelativeToNow =  TimeSpan.FromSeconds(45),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Priority = CacheItemPriority.High
                };   

                _memoryCache.Set(key, value, cacheEntryOptions);
            }

            return value;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
