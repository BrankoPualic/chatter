namespace Chatter.Common.Extensions;

public static class IEnumerableExtensions
{
	public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable) => enumerable ?? [];

	public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> enumerable) => enumerable?.Select((item, index) => (item, index));
}