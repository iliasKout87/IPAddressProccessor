using IPAddressProccessor.API.Services.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.Concrete
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache cache;
        private readonly IConfiguration config;

        public CacheProvider(
            IMemoryCache cache,
            IConfiguration config
            )
        {
            this.cache = cache;
            this.config = config;
        }

        public T GetFromCache<T>(string key) where T : class
        {
            var cachedValue = this.cache.Get(key);

            return cachedValue as T;
        }

        public void SetCache<T>(string key, T value) where T : class
        {
            var cacheSeconds = config.GetValue<int>("CacheTimeInSeconds");
            this.cache.Set(key, value, DateTimeOffset.Now.AddSeconds(cacheSeconds));
        }
    }
}
