using Chatter.Common;
using Chatter.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Chatter.Web.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex, logger);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ExceptionMiddleware> logger)
	{
		logger.LogError(ex, "{message}", ex.Message);
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		var response = ErrorConstants.ERROR_INTERNAL_ERROR;

		var json = response.SerializeJsonObject(new CamelCasePropertyNamesContractResolver(), formatting: Formatting.Indented);

		return context.Response.WriteAsync(json);
	}
}