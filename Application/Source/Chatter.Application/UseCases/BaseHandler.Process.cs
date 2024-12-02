using Chatter.Domain;
using Microsoft.Extensions.Logging;

namespace Chatter.Application.UseCases;

public abstract class BaseHandlerProcess
{
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