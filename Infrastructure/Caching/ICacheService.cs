namespace Infrastructure.Caching
{
	/// <summary>
	/// Interface for caching operations
	/// Supports multiple cache providers (Redis, InMemory, Distributed, etc.)
	/// </summary>
	public interface ICacheService
	{
		/// <summary>
		/// Get cached value by key
		/// </summary>
		Task<T?> GetAsync<T>(string key);

		/// <summary>
		/// Set cache value with expiration
		/// </summary>
		Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

		/// <summary>
		/// Remove cached value
		/// </summary>
		Task RemoveAsync(string key);

		/// <summary>
		/// Check if key exists
		/// </summary>
		Task<bool> ExistsAsync(string key);

		/// <summary>
		/// Clear all cache
		/// </summary>
		Task ClearAllAsync();
	}
}
