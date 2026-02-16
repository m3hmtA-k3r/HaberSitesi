namespace Shared.Responses
{
	/// <summary>
	/// Standardized API error response format
	/// </summary>
	public class ApiErrorResponse
	{
		public bool Success { get; set; } = false;
		public string Message { get; set; } = string.Empty;
		public string ErrorCode { get; set; } = string.Empty;
		public int StatusCode { get; set; }
		public string? TraceId { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		public IDictionary<string, string[]>? ValidationErrors { get; set; }

		public static ApiErrorResponse Create(string message, string errorCode, int statusCode, string? traceId = null)
		{
			return new ApiErrorResponse
			{
				Message = message,
				ErrorCode = errorCode,
				StatusCode = statusCode,
				TraceId = traceId
			};
		}

		public static ApiErrorResponse NotFound(string message, string? traceId = null)
		{
			return Create(message, "NOT_FOUND", 404, traceId);
		}

		public static ApiErrorResponse BadRequest(string message, string? traceId = null)
		{
			return Create(message, "BAD_REQUEST", 400, traceId);
		}

		public static ApiErrorResponse InternalError(string? traceId = null)
		{
			return Create("Beklenmeyen bir hata olustu. Lutfen daha sonra tekrar deneyin.", "INTERNAL_ERROR", 500, traceId);
		}

		public static ApiErrorResponse Validation(IDictionary<string, string[]> errors, string? traceId = null)
		{
			return new ApiErrorResponse
			{
				Message = "Dogrulama hatasi",
				ErrorCode = "VALIDATION_ERROR",
				StatusCode = 400,
				TraceId = traceId,
				ValidationErrors = errors
			};
		}
	}
}
