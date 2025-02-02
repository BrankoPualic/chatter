namespace Chatter.Common.Extensions;

public static class IEnumerableExtensions
{
	public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable) => enumerable ?? [];

	public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> enumerable) => enumerable?.Select((item, index) => (item, index));

	public static bool IsNullOrEmpty<T>(this IEnumerable<T> data) => data == null || !data.Any();

	public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> data) => data != null && data.Any();
}