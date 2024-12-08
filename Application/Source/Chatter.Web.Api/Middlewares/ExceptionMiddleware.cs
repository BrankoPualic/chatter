using Chatter.Application.Exceptions;
using Chatter.Common;
using Chatter.Common.Extensions;
using Newtonsoft.Json;
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
		context.Response.StatusCode = ex switch
		{
			FluentValidationException _ => (int)HttpStatusCode.UnprocessableEntity,
			_ => (int)HttpStatusCode.InternalServerError
		};

		var response = ErrorConstants.ERROR_INTERNAL_ERROR;

		if (ex is FluentValidationException validationException)
		{
			var errors = new Error();

			validationException.Failures.ToList().ForEach(_ => errors.AddErrors(_.Key, _.Value.ToList()));

			response = errors;
		}

		var json = response.SerializeJsonObject(formatting: Formatting.Indented);

		return context.Response.WriteAsync(json);
	}
}