namespace Chatter.Common;

public static class ErrorConstants
{
	public static readonly Error ERROR_NOT_FOUND = new(Constants.ERROR_NOT_FOUND);

	public static readonly Error ERROR_INVALID_OPERATION = new(Constants.ERROR_INVALID_OPERATION);

	public static readonly Error ERROR_UNAUTHORIZED = new(Constants.ERROR_UNAUTHORIZED);

	public static readonly Error ERROR_INTERNAL_ERROR = new(Constants.ERROR_INTERNAL_ERROR);
}