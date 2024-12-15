namespace Chatter.Application;

public class ResponseWrapper
{
	public ResponseWrapper()
	{
		IsSuccess = true;
		Errors = new();
	}

	public ResponseWrapper(Error error)
	{
		IsSuccess = false;
		Errors = error;
	}

	public bool IsSuccess { get; }

	public Error Errors { get; }

	public static ResponseWrapper<TResult> TryRun<TResult>(Func<TResult> func)
	{
		try
		{
			var result = func();
			return new(result);
		}
		catch (Exception e)
		{
			return new(new Error("Error", e.Message));
		}
	}

	public static async Task<ResponseWrapper> TryRunAsync(Task func)
	{
		try
		{
			await func;
			return new();
		}
		catch (Exception e)
		{
			return new(new Error("Error", e.Message));
		}
	}

	public static async Task<ResponseWrapper<TResult>> TryRunAsync<TResult>(Task<TResult> func)
	{
		try
		{
			var result = await func;
			return new(result);
		}
		catch (Exception e)
		{
			return new(new Error("Error", e.Message));
		}
	}

	public override string ToString() => !IsSuccess
		? $"ERRORS - {Errors}"
		: $"SUCCESS";
}