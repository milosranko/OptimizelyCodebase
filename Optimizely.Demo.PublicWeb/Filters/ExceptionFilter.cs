using System.Net;
using System.Text.Json;

namespace Optimizely.Demo.PublicWeb.Filters;

public class ExceptionFilter
{
	private readonly RequestDelegate _next;
	private readonly ILogger _logger;
	private readonly string _errorHandlingPath;

	public ExceptionFilter(RequestDelegate next, ILogger<ExceptionFilter> logger, string errorHandlingPath)
	{
		_next = next;
		_logger = logger;
		_errorHandlingPath = errorHandlingPath;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);

			if (context.Request.Path.StartsWithSegments("/api"))
				await HandleApiExceptionAsync(context, ex);
			else
				HandleException(context);
		}
	}

	private async Task HandleApiExceptionAsync(HttpContext context, Exception exception)
	{
		var code = HttpStatusCode.InternalServerError;
		var message = exception.Message;

		switch (exception)
		{
			//TODO add supported exceptions
			case KeyNotFoundException _:
				code = HttpStatusCode.NotFound;
				break;
			case UnauthorizedAccessException _:
				code = HttpStatusCode.Forbidden;
				break;
			case ArgumentException _:
			case OperationCanceledException _:
			case Exception _:
				code = HttpStatusCode.InternalServerError;
				break;
		}

		var result = new { Code = code, Message = message };
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)code;

		await context.Response.WriteAsJsonAsync(
			result,
			new JsonSerializerOptions
			{
				PropertyNamingPolicy = null,
				PropertyNameCaseInsensitive = true
			});
	}

	private void HandleException(HttpContext context)
	{
		context.Request.Path = _errorHandlingPath;
	}
}
