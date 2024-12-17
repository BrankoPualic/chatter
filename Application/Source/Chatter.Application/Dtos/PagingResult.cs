namespace Chatter.Application.Dtos;

public class PagingResult<TData>
{
	public IEnumerable<TData> Data { get; set; } = [];

	public long Total { get; set; }
}