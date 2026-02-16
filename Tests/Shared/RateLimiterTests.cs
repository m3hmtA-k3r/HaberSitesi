using Microsoft.Extensions.Caching.Memory;
using Shared.RateLimiting;
using Xunit;

namespace MASKER.Tests.Shared
{
	public class RateLimiterTests
	{
		private readonly IMemoryCache _cache;

		public RateLimiterTests()
		{
			_cache = new MemoryCache(new MemoryCacheOptions());
		}

		[Fact]
		public void IsAllowed_IlkIstek_TrueDoner()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test", maxRequests: 3, window: TimeSpan.FromMinutes(1));

			// Act
			var result = limiter.IsAllowed("client1");

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void IsAllowed_LimitAsilmadi_TrueDoner()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test2", maxRequests: 3, window: TimeSpan.FromMinutes(1));

			// Act & Assert
			Assert.True(limiter.IsAllowed("client2"));
			Assert.True(limiter.IsAllowed("client2"));
			Assert.True(limiter.IsAllowed("client2"));
		}

		[Fact]
		public void IsAllowed_LimitAsildi_FalseDoner()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test3", maxRequests: 2, window: TimeSpan.FromMinutes(1));

			// Act
			limiter.IsAllowed("client3");
			limiter.IsAllowed("client3");
			var result = limiter.IsAllowed("client3");

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void IsAllowed_FarkliClientler_BagimsizCalisir()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test4", maxRequests: 1, window: TimeSpan.FromMinutes(1));

			// Act
			limiter.IsAllowed("clientA");
			var resultA = limiter.IsAllowed("clientA");
			var resultB = limiter.IsAllowed("clientB");

			// Assert
			Assert.False(resultA); // clientA limiti asti
			Assert.True(resultB);  // clientB bagimsiz
		}

		[Fact]
		public void GetRemainingRequests_DogruKalanSayiDoner()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test5", maxRequests: 5, window: TimeSpan.FromMinutes(1));

			// Act
			limiter.IsAllowed("client5");
			limiter.IsAllowed("client5");
			var remaining = limiter.GetRemainingRequests("client5");

			// Assert
			Assert.Equal(3, remaining);
		}

		[Fact]
		public void GetRemainingRequests_HicIstekYok_MaxDoner()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test6", maxRequests: 5, window: TimeSpan.FromMinutes(1));

			// Act
			var remaining = limiter.GetRemainingRequests("newclient");

			// Assert
			Assert.Equal(5, remaining);
		}

		[Fact]
		public void GetTimeUntilReset_IstekVar_ZamanDoner()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test7", maxRequests: 3, window: TimeSpan.FromMinutes(5));

			// Act
			limiter.IsAllowed("client7");
			var resetTime = limiter.GetTimeUntilReset("client7");

			// Assert
			Assert.NotNull(resetTime);
			Assert.True(resetTime.Value.TotalMinutes <= 5);
			Assert.True(resetTime.Value.TotalMinutes > 0);
		}

		[Fact]
		public void GetTimeUntilReset_HicIstekYok_NullDoner()
		{
			// Arrange
			var limiter = new RateLimiter(_cache, "test8", maxRequests: 3, window: TimeSpan.FromMinutes(1));

			// Act
			var resetTime = limiter.GetTimeUntilReset("unknownclient");

			// Assert
			Assert.Null(resetTime);
		}
	}
}
