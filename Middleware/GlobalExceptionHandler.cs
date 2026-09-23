using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SAMS.Middleware
{
	public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger = logger;

		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			var (statusCode, errorCode, title, detail) = MapException(exception);
			var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

			_logger.LogError(
				exception,
				"Unhandled exception occurred. ErrorCode: {ErrorCode}, TraceId: {TraceId}, Path: {Path}",
				errorCode,
				traceId,
				httpContext.Request.Path);

			var problemDetails = new ProblemDetails
			{
				Status = statusCode,
				Title = title,
				Detail = detail,
				Type = $"https://httpstatuses.com/{statusCode}",
				Instance = httpContext.Request.Path
			};

			problemDetails.Extensions["code"] = errorCode;
			problemDetails.Extensions["traceId"] = traceId;
			problemDetails.Extensions["timestampUtc"] = DateTime.UtcNow;

			httpContext.Response.StatusCode = statusCode;
			httpContext.Response.ContentType = "application/problem+json";

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
			return true;
		}

		private static (int StatusCode, string ErrorCode, string Title, string Detail) MapException(Exception exception)
		{
			return exception switch
			{
				ArgumentException => (
					StatusCodes.Status400BadRequest,
					"SAMS_BAD_REQUEST_001",
					"Invalid Request",
					exception.Message),

				KeyNotFoundException => (
					StatusCodes.Status404NotFound,
					"SAMS_NOT_FOUND_001",
					"Resource Not Found",
					exception.Message),

				UnauthorizedAccessException => (
					StatusCodes.Status401Unauthorized,
					"SAMS_AUTH_001",
					"Unauthorized",
					"You are not authorized to perform this action."),

				NotImplementedException => (
					StatusCodes.Status501NotImplemented,
					"SAMS_NOT_IMPLEMENTED_001",
					"Not Implemented",
					"This functionality is not yet implemented."),

				_ => (
					StatusCodes.Status500InternalServerError,
					"SAMS_SERVER_001",
					"Unexpected Server Error",
					"An unexpected error occurred. Please contact support with the provided trace identifier.")
			};
		}
	}
}
