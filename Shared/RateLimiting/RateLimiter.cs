using Microsoft.Extensions.Caching.Memory;

namespace Shared.RateLimiting
{
	/// <summary>
	/// Simple in-memory rate limiter for protecting forms from spam/abuse
	/// Uses sliding window algorithm
	/// </summary>
	public class RateLimiter
	{
		private readonly IMemoryCache _cache;
		private readonly int _maxRequests;
		private readonly TimeSpan _window;
		private readonly string _prefix;

		public RateLimiter(IMemoryCache cache, string prefix, int maxRequests, TimeSpan window)
		{
			_cache = cache;
			_prefix = prefix;
			_maxRequests = maxRequests;
			_window = window;
		}

		/// <summary>
		/// Check if request is allowed for the given key (e.g., IP address)
		/// </summary>
		/// <param name="key">Unique identifier for the client (IP address, user ID, etc.)</param>
		/// <returns>True if request is allowed, false if rate limit exceeded</returns>
		public bool IsAllowed(string key)
		{
			var cacheKey = $"{_prefix}:{key}";
			var now = DateTime.UtcNow;

			if (!_cache.TryGetValue(cacheKey, out List<DateTime>? timestamps))
			{
				timestamps = new List<DateTime>();
			}

			// Remove expired timestamps
			timestamps = timestamps!.Where(t => now - t < _window).ToList();

			// Check if limit exceeded
			if (timestamps.Count >= _maxRequests)
			{
				return false;
			}

			// Record this request
			timestamps.Add(now);
			_cache.Set(cacheKey, timestamps, _window);

			return true;
		}

		/// <summary>
		/// Get remaining requests for the given key
		/// </summary>
		public int GetRemainingRequests(string key)
		{
			var cacheKey = $"{_prefix}:{key}";
			var now = DateTime.UtcNow;

			if (!_cache.TryGetValue(cacheKey, out List<DateTime>? timestamps))
			{
				return _maxRequests;
			}

			var validCount = timestamps!.Count(t => now - t < _window);
			return Math.Max(0, _maxRequests - validCount);
		}

		/// <summary>
		/// Get time until rate limit resets for the given key
		/// </summary>
		public TimeSpan? GetTimeUntilReset(string key)
		{
			var cacheKey = $"{_prefix}:{key}";
			var now = DateTime.UtcNow;

			if (!_cache.TryGetValue(cacheKey, out List<DateTime>? timestamps) || timestamps!.Count == 0)
			{
				return null;
			}

			var oldestValid = timestamps.Where(t => now - t < _window).OrderBy(t => t).FirstOrDefault();
			if (oldestValid == default)
			{
				return null;
			}

			return _window - (now - oldestValid);
		}
	}
}
