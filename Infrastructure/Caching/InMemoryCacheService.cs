using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Infrastructure.Caching
{
	/// <summary>
	/// In-memory cache implementation
	/// Fast but limited to single server (not distributed)
	/// </summary>
	public class InMemoryCacheService : ICacheService
	{
		private readonly IMemoryCache _cache;
		private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);

		public InMemoryCacheService(IMemoryCache cache)
		{
			_cache = cache;
		}

		public Task<T?> GetAsync<T>(string key)
		{
			if (_cache.TryGetValue(key, out string? jsonValue) && jsonValue != null)
			{
				var value = JsonSerializer.Deserialize<T>(jsonValue);
				return Task.FromResult(value);
			}

			return Task.FromResult<T?>(default);
		}

		public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			var jsonValue = JsonSerializer.Serialize(value);
			var cacheExpiration = expiration ?? _defaultExpiration;

			_cache.Set(key, jsonValue, cacheExpiration);

			return Task.CompletedTask;
		}

		public Task RemoveAsync(string key)
		{
			_cache.Remove(key);
			return Task.CompletedTask;
		}

		public Task<bool> ExistsAsync(string key)
		{
			var exists = _cache.TryGetValue(key, out _);
			return Task.FromResult(exists);
		}

		public Task ClearAllAsync()
		{
			// MemoryCache doesn't have a built-in clear all method
			// In production, consider using IMemoryCache as a field and disposing/recreating
			// For now, this is a limitation
			return Task.CompletedTask;
		}
	}
}
