namespace Chatter.Domain.Models.Application;

public class PagingResult<TModel>
{
	public IEnumerable<TModel> Data { get; set; } = [];

	public long Total { get; set; }
}