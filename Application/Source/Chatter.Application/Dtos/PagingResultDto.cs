namespace Chatter.Application.Dtos;

public class PagingResultDto<TData>
{
	public IEnumerable<TData> Data { get; set; }

	public long Total { get; set; }
}