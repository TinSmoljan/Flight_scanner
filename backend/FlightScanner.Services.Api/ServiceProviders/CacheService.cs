using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FlightScanner.Services.Api.Services
{
	public class CacheService : ICacheService
	{
		private readonly IDistributedCache _cache;
		private readonly ILogger<CacheService> _logger;
		private readonly int _cacheDuration;

		public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
		{
			_cache = cache;
			_cacheDuration = 10;  //minutes
			_logger = logger;
		}

		public async Task<bool> SetItem(string key, object value, TimeSpan? slidingExpirtion = null, DateTimeOffset? absoluteExpiration = null)
		{
			var json = JsonConvert.SerializeObject(value);
			slidingExpirtion = !slidingExpirtion.HasValue && !absoluteExpiration.HasValue ? TimeSpan.FromMinutes(_cacheDuration) : slidingExpirtion;

			try
			{
				await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions()
				{
					SlidingExpiration = slidingExpirtion,
					AbsoluteExpiration = absoluteExpiration
				});

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Failed to set cache item with key: {key} and value: {value}");
				return false;
			}
		}

		public async Task<T> GetItem<T>(string key)
		{
			try
			{
				var json = await _cache.GetStringAsync(key);
				if (json != null)
				{
					return JsonConvert.DeserializeObject<T>(json);
				}

				return default;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Failed to get cache item with key: {key}");
				return await Task.FromResult(default(T));
			}
		}

		public async Task<bool> RemoveItem(string key)
		{
			try
			{
				await _cache.RemoveAsync(key);
				return true;
			}
			catch
			{
				return await Task.FromResult(false);
			}
		}
	}
}
