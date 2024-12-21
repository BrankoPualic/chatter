using Chatter.Domain.Models;

namespace Chatter.Application.Extensions;

public static class ListExtensions
{
	public static List<Guid> SelectIds<T>(this IEnumerable<T> data) where T : IBaseDomain => data.Select(_ => _.Id).ToList();
}