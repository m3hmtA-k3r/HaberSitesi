using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
	/// <summary>
	/// JWT token service implementation
	/// Generates and validates JWT tokens for authentication
	/// </summary>
	public class JwtTokenService : IJwtTokenService
	{
		private readonly string _secretKey;
		private readonly string _issuer;
		private readonly string _audience;
		private readonly int _expirationMinutes;

		public JwtTokenService(IConfiguration configuration)
		{
			// Environment variable takes priority over configuration
			_secretKey = Environment.GetEnvironmentVariable("MASKER_JWT_SECRET")
				?? configuration["JwtSettings:SecretKey"]
				?? throw new ArgumentNullException("JWT SecretKey is not configured. Set MASKER_JWT_SECRET environment variable.");
			_issuer = Environment.GetEnvironmentVariable("MASKER_JWT_ISSUER")
				?? configuration["JwtSettings:Issuer"]
				?? "MaskerAPI";
			_audience = Environment.GetEnvironmentVariable("MASKER_JWT_AUDIENCE")
				?? configuration["JwtSettings:Audience"]
				?? "MaskerClients";
			_expirationMinutes = int.Parse(
				Environment.GetEnvironmentVariable("MASKER_JWT_EXPIRATION_MINUTES")
				?? configuration["JwtSettings:ExpirationMinutes"]
				?? "60");
		}

		public string GenerateToken(int userId, string email, string fullName)
		{
			return GenerateToken(userId, email, fullName, Array.Empty<string>());
		}

		public string GenerateToken(int userId, string email, string fullName, IEnumerable<string> roles)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_secretKey);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
				new Claim(ClaimTypes.Email, email),
				new Claim(ClaimTypes.Name, fullName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			// Add role claims
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
				Issuer = _issuer,
				Audience = _audience,
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public int? ValidateToken(string token)
		{
			if (string.IsNullOrEmpty(token))
				return null;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_secretKey);

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = _issuer,
					ValidateAudience = true,
					ValidAudience = _audience,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

				return userId;
			}
			catch
			{
				return null;
			}
		}

		public DateTime GetTokenExpiration()
		{
			return DateTime.UtcNow.AddMinutes(_expirationMinutes);
		}
	}
}
