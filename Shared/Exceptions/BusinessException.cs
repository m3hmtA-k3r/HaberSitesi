namespace Shared.Exceptions
{
	/// <summary>
	/// Custom exception for business logic errors
	/// </summary>
	public class BusinessException : Exception
	{
		public int StatusCode { get; }
		public string ErrorCode { get; }

		public BusinessException(string message, int statusCode = 400, string errorCode = "BUSINESS_ERROR")
			: base(message)
		{
			StatusCode = statusCode;
			ErrorCode = errorCode;
		}

		public BusinessException(string message, Exception innerException, int statusCode = 400, string errorCode = "BUSINESS_ERROR")
			: base(message, innerException)
		{
			StatusCode = statusCode;
			ErrorCode = errorCode;
		}
	}

	/// <summary>
	/// Exception for resource not found errors
	/// </summary>
	public class NotFoundException : BusinessException
	{
		public NotFoundException(string resourceName, object key)
			: base($"{resourceName} bulunamadi (ID: {key})", 404, "NOT_FOUND")
		{
		}

		public NotFoundException(string message)
			: base(message, 404, "NOT_FOUND")
		{
		}
	}

	/// <summary>
	/// Exception for validation errors
	/// </summary>
	public class ValidationException : BusinessException
	{
		public IDictionary<string, string[]>? Errors { get; }

		public ValidationException(string message)
			: base(message, 400, "VALIDATION_ERROR")
		{
		}

		public ValidationException(IDictionary<string, string[]> errors)
			: base("Dogrulama hatasi", 400, "VALIDATION_ERROR")
		{
			Errors = errors;
		}
	}

	/// <summary>
	/// Exception for unauthorized access
	/// </summary>
	public class UnauthorizedException : BusinessException
	{
		public UnauthorizedException(string message = "Bu islemi yapmaya yetkiniz yok")
			: base(message, 401, "UNAUTHORIZED")
		{
		}
	}

	/// <summary>
	/// Exception for forbidden access
	/// </summary>
	public class ForbiddenException : BusinessException
	{
		public ForbiddenException(string message = "Bu kaynaga erisim izniniz yok")
			: base(message, 403, "FORBIDDEN")
		{
		}
	}
}
