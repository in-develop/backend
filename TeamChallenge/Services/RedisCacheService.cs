using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace TeamChallenge.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        public RedisCacheService(IDistributedCache cache,
            ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5),
            };
        }
        private string GenerateKey<T>(string id) => $"{typeof(T).Name}_{id}";
        public async Task<T?> GetValueAsync<T>(int id) where T : class
        {
            return await GetValueAsync<T>(id.ToString());
        }

        public async Task<T?> GetValueAsync<T>(string id) where T : class
        {
            var key = GenerateKey<T>(id);
            var cachedBytes = await _cache.GetAsync(key);

            _logger.LogInformation("Retrieving data from cache with key: {0}", key);
            if (cachedBytes == null)
            {
                _logger.LogInformation("No data found in cache with key: {0}", key);
                return null;
            }

            var data = JsonSerializer.Deserialize<T>(cachedBytes);
            return data;
        }
        public async Task SetValueAsync<T>(T data, int id)
        {
            await SetValueAsync(data, id.ToString());
        }

        public async Task SetValueAsync<T>(T data, string id)
        {
            var key = GenerateKey<T>(id);

            var dataBytes = JsonSerializer.SerializeToUtf8Bytes(data);
            await _cache.SetAsync(key, dataBytes, _cacheOptions);

            _logger.LogInformation("Data cached with key: {0}", key);
        }

        public async Task RemoveValueAsync<T>(int id)
        {
            await RemoveValueAsync<T>(id.ToString());
        }

        public async Task RemoveValueAsync<T>(string id)
        {
            var key = GenerateKey<T>(id);
            await _cache.RemoveAsync(key);

            _logger.LogInformation("Data removed from cache with key: {0}", key);
        }
    }

}
