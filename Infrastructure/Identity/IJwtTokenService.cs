namespace Infrastructure.Identity
{
	/// <summary>
	/// Interface for JWT token operations
	/// </summary>
	public interface IJwtTokenService
	{
		/// <summary>
		/// Generate JWT token for authenticated user
		/// </summary>
		string GenerateToken(int userId, string email, string fullName);

		/// <summary>
		/// Generate JWT token for authenticated user with roles
		/// </summary>
		string GenerateToken(int userId, string email, string fullName, IEnumerable<string> roles);

		/// <summary>
		/// Validate JWT token and return user ID
		/// </summary>
		int? ValidateToken(string token);

		/// <summary>
		/// Get token expiration time
		/// </summary>
		DateTime GetTokenExpiration();
	}
}
