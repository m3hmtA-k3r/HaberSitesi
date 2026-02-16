using Shared.Exceptions;
using Shared.Responses;
using Xunit;

namespace MASKER.Tests.Shared
{
	public class ExceptionTests
	{
		[Fact]
		public void BusinessException_DogruDegerlerIleOlusturulur()
		{
			// Arrange & Act
			var ex = new BusinessException("Test hatasi", 400, "TEST_ERROR");

			// Assert
			Assert.Equal("Test hatasi", ex.Message);
			Assert.Equal(400, ex.StatusCode);
			Assert.Equal("TEST_ERROR", ex.ErrorCode);
		}

		[Fact]
		public void NotFoundException_DogruMesajOlusturur()
		{
			// Arrange & Act
			var ex = new NotFoundException("Haber", 123);

			// Assert
			Assert.Contains("Haber", ex.Message);
			Assert.Contains("123", ex.Message);
			Assert.Equal(404, ex.StatusCode);
			Assert.Equal("NOT_FOUND", ex.ErrorCode);
		}

		[Fact]
		public void ValidationException_HatalarIleDuzgunCalisir()
		{
			// Arrange
			var errors = new Dictionary<string, string[]>
			{
				{ "Email", new[] { "Gecersiz email formati" } },
				{ "Ad", new[] { "Ad alani zorunludur" } }
			};

			// Act
			var ex = new ValidationException(errors);

			// Assert
			Assert.Equal(400, ex.StatusCode);
			Assert.Equal("VALIDATION_ERROR", ex.ErrorCode);
			Assert.NotNull(ex.Errors);
			Assert.Equal(2, ex.Errors.Count);
		}

		[Fact]
		public void UnauthorizedException_VarsayilanMesajKullanir()
		{
			// Arrange & Act
			var ex = new UnauthorizedException();

			// Assert
			Assert.Equal(401, ex.StatusCode);
			Assert.Equal("UNAUTHORIZED", ex.ErrorCode);
			Assert.Contains("yetki", ex.Message.ToLower());
		}

		[Fact]
		public void ForbiddenException_VarsayilanMesajKullanir()
		{
			// Arrange & Act
			var ex = new ForbiddenException();

			// Assert
			Assert.Equal(403, ex.StatusCode);
			Assert.Equal("FORBIDDEN", ex.ErrorCode);
			Assert.Contains("erisim", ex.Message.ToLower());
		}
	}

	public class ApiErrorResponseTests
	{
		[Fact]
		public void Create_DogruDegerlerDoner()
		{
			// Arrange & Act
			var response = ApiErrorResponse.Create("Test mesaji", "TEST_CODE", 400, "trace-123");

			// Assert
			Assert.False(response.Success);
			Assert.Equal("Test mesaji", response.Message);
			Assert.Equal("TEST_CODE", response.ErrorCode);
			Assert.Equal(400, response.StatusCode);
			Assert.Equal("trace-123", response.TraceId);
		}

		[Fact]
		public void NotFound_404Doner()
		{
			// Arrange & Act
			var response = ApiErrorResponse.NotFound("Kaynak bulunamadi");

			// Assert
			Assert.Equal(404, response.StatusCode);
			Assert.Equal("NOT_FOUND", response.ErrorCode);
		}

		[Fact]
		public void BadRequest_400Doner()
		{
			// Arrange & Act
			var response = ApiErrorResponse.BadRequest("Gecersiz istek");

			// Assert
			Assert.Equal(400, response.StatusCode);
			Assert.Equal("BAD_REQUEST", response.ErrorCode);
		}

		[Fact]
		public void InternalError_500Doner()
		{
			// Arrange & Act
			var response = ApiErrorResponse.InternalError("trace-456");

			// Assert
			Assert.Equal(500, response.StatusCode);
			Assert.Equal("INTERNAL_ERROR", response.ErrorCode);
			Assert.Equal("trace-456", response.TraceId);
		}

		[Fact]
		public void Validation_HatalarIIcerir()
		{
			// Arrange
			var errors = new Dictionary<string, string[]>
			{
				{ "Field1", new[] { "Error1", "Error2" } }
			};

			// Act
			var response = ApiErrorResponse.Validation(errors);

			// Assert
			Assert.Equal(400, response.StatusCode);
			Assert.Equal("VALIDATION_ERROR", response.ErrorCode);
			Assert.NotNull(response.ValidationErrors);
			Assert.Single(response.ValidationErrors);
		}
	}
}
