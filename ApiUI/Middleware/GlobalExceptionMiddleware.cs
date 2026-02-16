using System.Net;
using System.Text.Json;
using Shared.Exceptions;
using Shared.Responses;

namespace ApiUI.Middleware
{
	/// <summary>
	/// Global exception handling middleware for consistent error responses
	/// </summary>
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var traceId = context.TraceIdentifier;
			ApiErrorResponse response;
			int statusCode;

			switch (exception)
			{
				case NotFoundException notFoundEx:
					statusCode = (int)HttpStatusCode.NotFound;
					response = ApiErrorResponse.Create(notFoundEx.Message, notFoundEx.ErrorCode, statusCode, traceId);
					_logger.LogWarning("Resource not found: {Message} | TraceId: {TraceId}", notFoundEx.Message, traceId);
					break;

				case ValidationException validationEx:
					statusCode = (int)HttpStatusCode.BadRequest;
					response = validationEx.Errors != null
						? ApiErrorResponse.Validation(validationEx.Errors, traceId)
						: ApiErrorResponse.Create(validationEx.Message, validationEx.ErrorCode, statusCode, traceId);
					_logger.LogWarning("Validation error: {Message} | TraceId: {TraceId}", validationEx.Message, traceId);
					break;

				case UnauthorizedException unauthorizedEx:
					statusCode = (int)HttpStatusCode.Unauthorized;
					response = ApiErrorResponse.Create(unauthorizedEx.Message, unauthorizedEx.ErrorCode, statusCode, traceId);
					_logger.LogWarning("Unauthorized access: {Message} | TraceId: {TraceId}", unauthorizedEx.Message, traceId);
					break;

				case ForbiddenException forbiddenEx:
					statusCode = (int)HttpStatusCode.Forbidden;
					response = ApiErrorResponse.Create(forbiddenEx.Message, forbiddenEx.ErrorCode, statusCode, traceId);
					_logger.LogWarning("Forbidden access: {Message} | TraceId: {TraceId}", forbiddenEx.Message, traceId);
					break;

				case BusinessException businessEx:
					statusCode = businessEx.StatusCode;
					response = ApiErrorResponse.Create(businessEx.Message, businessEx.ErrorCode, statusCode, traceId);
					_logger.LogWarning("Business error: {Message} | TraceId: {TraceId}", businessEx.Message, traceId);
					break;

				default:
					statusCode = (int)HttpStatusCode.InternalServerError;
					response = ApiErrorResponse.InternalError(traceId);

					// Log full exception details for internal errors
					_logger.LogError(exception,
						"Unhandled exception | TraceId: {TraceId} | Path: {Path} | Method: {Method}",
						traceId, context.Request.Path, context.Request.Method);

					// Include exception details in development mode
					if (_env.IsDevelopment())
					{
						response.Message = exception.Message;
					}
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;

			var jsonOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = _env.IsDevelopment()
			};

			var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
			await context.Response.WriteAsync(jsonResponse);
		}
	}

	/// <summary>
	/// Extension method to register the middleware
	/// </summary>
	public static class GlobalExceptionMiddlewareExtensions
	{
		public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
		{
			return app.UseMiddleware<GlobalExceptionMiddleware>();
		}
	}
}
