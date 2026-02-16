using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Caching
{
	/// <summary>
	/// Hybrid cache service that uses Redis as primary and InMemory as fallback
	/// Provides resilience when Redis is unavailable
	/// </summary>
	public class HybridCacheService : ICacheService, IDisposable
	{
		private readonly RedisCacheService? _redisCache;
		private readonly InMemoryCacheService _memoryCache;
		private readonly ILogger<HybridCacheService> _logger;
		private readonly bool _redisEnabled;
		private bool _disposed;

		public HybridCacheService(
			IConfiguration configuration,
			IMemoryCache memoryCache,
			ILogger<HybridCacheService> logger,
			ILogger<RedisCacheService> redisLogger)
		{
			_logger = logger;
			_memoryCache = new InMemoryCacheService(memoryCache);

			// Check if Redis is enabled
			_redisEnabled = configuration.GetValue<bool>("Redis:Enabled", false)
				|| !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MASKER_REDIS_CONNECTION"));

			if (_redisEnabled)
			{
				try
				{
					_redisCache = new RedisCacheService(configuration, redisLogger);
					_logger.LogInformation("Hybrid cache initialized with Redis primary and InMemory fallback");
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Failed to initialize Redis, falling back to InMemory cache only");
					_redisEnabled = false;
				}
			}
			else
			{
				_logger.LogInformation("Hybrid cache initialized with InMemory cache only (Redis disabled)");
			}
		}

		public async Task<T?> GetAsync<T>(string key)
		{
			// Try Redis first if available
			if (_redisEnabled && _redisCache?.IsConnected == true)
			{
				try
				{
					var value = await _redisCache.GetAsync<T>(key);
					if (value != null)
					{
						return value;
					}
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Redis GET failed, falling back to InMemory for key {Key}", key);
				}
			}

			// Fallback to InMemory
			return await _memoryCache.GetAsync<T>(key);
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			// Always set in InMemory for local access
			await _memoryCache.SetAsync(key, value, expiration);

			// Also set in Redis if available
			if (_redisEnabled && _redisCache?.IsConnected == true)
			{
				try
				{
					await _redisCache.SetAsync(key, value, expiration);
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Redis SET failed for key {Key}", key);
				}
			}
		}

		public async Task RemoveAsync(string key)
		{
			await _memoryCache.RemoveAsync(key);

			if (_redisEnabled && _redisCache?.IsConnected == true)
			{
				try
				{
					await _redisCache.RemoveAsync(key);
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Redis DELETE failed for key {Key}", key);
				}
			}
		}

		public async Task<bool> ExistsAsync(string key)
		{
			if (_redisEnabled && _redisCache?.IsConnected == true)
			{
				try
				{
					if (await _redisCache.ExistsAsync(key))
						return true;
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Redis EXISTS failed for key {Key}", key);
				}
			}

			return await _memoryCache.ExistsAsync(key);
		}

		public async Task ClearAllAsync()
		{
			await _memoryCache.ClearAllAsync();

			if (_redisEnabled && _redisCache?.IsConnected == true)
			{
				try
				{
					await _redisCache.ClearAllAsync();
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Redis CLEAR ALL failed");
				}
			}
		}

		/// <summary>
		/// Get current cache status for health checks
		/// </summary>
		public CacheStatus GetStatus()
		{
			return new CacheStatus
			{
				RedisEnabled = _redisEnabled,
				RedisConnected = _redisCache?.IsConnected ?? false,
				InMemoryEnabled = true
			};
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_redisCache?.Dispose();
				}
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	/// <summary>
	/// Cache status for monitoring
	/// </summary>
	public class CacheStatus
	{
		public bool RedisEnabled { get; set; }
		public bool RedisConnected { get; set; }
		public bool InMemoryEnabled { get; set; }
	}
}
