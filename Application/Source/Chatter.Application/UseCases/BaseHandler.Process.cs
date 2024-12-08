namespace Chatter.Application.UseCases;

public abstract class BaseHandlerProcess
{
	public static readonly Error ERROR_NOT_FOUND = ErrorConstants.ERROR_NOT_FOUND;
	public static readonly Error ERROR_INVALID_OPERATION = ErrorConstants.ERROR_INVALID_OPERATION;
	public static readonly Error ERROR_UNAUTHORIZED = ErrorConstants.ERROR_UNAUTHORIZED;
	public static readonly Error ERROR_INTERNAL_ERROR = ErrorConstants.ERROR_INTERNAL_ERROR;

	protected static void TryRun(Action action, ILogger logger)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, ex.Message);
			throw;
		}
	}

	protected static async Task TryRunAsync(Func<Task> action, ILogger logger)
	{
		try
		{
			await action();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, ex.Message);
			throw;
		}
	}

	protected static void CheckUserAuthorization(IIdentityUser user, params eSystemRole[] requiredRoles)
	{
		if (!user.IsAuthenticated || !user.HasRole([.. requiredRoles]))
		{
			throw new UnauthorizedAccessException();
		}
	}
}