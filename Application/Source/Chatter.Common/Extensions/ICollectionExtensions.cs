namespace Chatter.Common.Extensions;

public static class ICollectionExtensions
{
	public static void RemoveRange<T>(this ICollection<T> data, List<T> removing) where T : class => removing.ForEach(_ => data.Remove(_));

	public static void AddRange<T>(this ICollection<T> data, List<T> adding) where T : class => adding.ForEach(_ => data.Add(_));
}