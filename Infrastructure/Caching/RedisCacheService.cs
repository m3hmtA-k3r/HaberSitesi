using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Caching
{
	/// <summary>
	/// Redis cache implementation for distributed caching
	/// Supports horizontal scaling across multiple servers
	/// </summary>
	public class RedisCacheService : ICacheService, IDisposable
	{
		private readonly IConnectionMultiplexer _redis;
		private readonly IDatabase _db;
		private readonly ILogger<RedisCacheService> _logger;
		private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);
		private readonly string _keyPrefix;
		private bool _disposed;

		public RedisCacheService(IConfiguration configuration, ILogger<RedisCacheService> logger)
		{
			_logger = logger;

			var connectionString = Environment.GetEnvironmentVariable("MASKER_REDIS_CONNECTION")
				?? configuration["Redis:ConnectionString"]
				?? "localhost:6379";

			_keyPrefix = configuration["Redis:KeyPrefix"] ?? "masker:";

			try
			{
				var options = ConfigurationOptions.Parse(connectionString);
				options.AbortOnConnectFail = false;
				options.ConnectRetry = 3;
				options.ConnectTimeout = 5000;

				_redis = ConnectionMultiplexer.Connect(options);
				_db = _redis.GetDatabase();

				_logger.LogInformation("Redis cache connected successfully to {ConnectionString}", connectionString);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to connect to Redis at {ConnectionString}", connectionString);
				throw;
			}
		}

		private string GetPrefixedKey(string key) => $"{_keyPrefix}{key}";

		public async Task<T?> GetAsync<T>(string key)
		{
			try
			{
				var prefixedKey = GetPrefixedKey(key);
				var value = await _db.StringGetAsync(prefixedKey);

				if (value.IsNullOrEmpty)
				{
					return default;
				}

				return JsonSerializer.Deserialize<T>(value!);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Redis GET failed for key {Key}", key);
				return default;
			}
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			try
			{
				var prefixedKey = GetPrefixedKey(key);
				var jsonValue = JsonSerializer.Serialize(value);
				var cacheExpiration = expiration ?? _defaultExpiration;

				await _db.StringSetAsync(prefixedKey, jsonValue, cacheExpiration);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Redis SET failed for key {Key}", key);
			}
		}

		public async Task RemoveAsync(string key)
		{
			try
			{
				var prefixedKey = GetPrefixedKey(key);
				await _db.KeyDeleteAsync(prefixedKey);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Redis DELETE failed for key {Key}", key);
			}
		}

		public async Task<bool> ExistsAsync(string key)
		{
			try
			{
				var prefixedKey = GetPrefixedKey(key);
				return await _db.KeyExistsAsync(prefixedKey);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Redis EXISTS failed for key {Key}", key);
				return false;
			}
		}

		public async Task ClearAllAsync()
		{
			try
			{
				var server = _redis.GetServer(_redis.GetEndPoints().First());
				var keys = server.Keys(pattern: $"{_keyPrefix}*");

				foreach (var key in keys)
				{
					await _db.KeyDeleteAsync(key);
				}

				_logger.LogInformation("Redis cache cleared for prefix {Prefix}", _keyPrefix);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Redis CLEAR ALL failed");
			}
		}

		/// <summary>
		/// Check if Redis connection is healthy
		/// </summary>
		public bool IsConnected => _redis?.IsConnected ?? false;

		/// <summary>
		/// Get Redis server info for monitoring
		/// </summary>
		public async Task<string> GetServerInfoAsync()
		{
			try
			{
				var server = _redis.GetServer(_redis.GetEndPoints().First());
				var info = await server.InfoAsync();
				return string.Join(Environment.NewLine, info.SelectMany(g => g.Select(p => $"{p.Key}: {p.Value}")));
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Failed to get Redis server info");
				return "Unable to retrieve server info";
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_redis?.Dispose();
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
}
