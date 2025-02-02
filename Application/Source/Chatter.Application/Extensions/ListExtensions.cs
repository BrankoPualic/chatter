using Chatter.Domain.Models;

namespace Chatter.Application.Extensions;

public static class ListExtensions
{
	public static List<Guid> SelectIds<T>(this IEnumerable<T> data) where T : IBaseDomain => data.Select(_ => _.Id).ToList();

	public static List<Guid> SelectIds<T>(this IEnumerable<T> data, Func<T, Guid?> selector) => data.Select(selector).WhereNotNull().Select(_ => _.Value).ToList();

	public static List<T> WhereNotNull<T>(this IEnumerable<T> data) => data.Where(_ => _ != null).ToList();
}