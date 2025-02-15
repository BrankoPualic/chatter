namespace Chatter.Application.Search;

public class CommentSearchOptions : SearchOptions
{
	public Guid PostId { get; set; }

	public Guid? ParentId { get; set; }
}