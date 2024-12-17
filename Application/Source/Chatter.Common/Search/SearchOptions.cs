namespace Chatter.Common.Search;

public class SearchOptions
{
	public int Skip { get; set; }

	public int Take { get; set; }

	public string Filter { get; set; }

	public bool? TotalCount { get; set; }
}