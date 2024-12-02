namespace Chatter.Common.Extensions;

public static class DataExtensions
{
	public static bool IsNullOrEmpty<T>(this T data) where T : class => data == null || EqualityComparer<T>.Default.Equals(data, default);

	public static bool IsNotNullOrEmpty<T>(this T data) where T : class => !data.IsNullOrEmpty();
}