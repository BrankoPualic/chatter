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
}