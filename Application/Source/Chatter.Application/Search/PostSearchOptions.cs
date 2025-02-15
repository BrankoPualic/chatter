using Chatter.Common.Search;

namespace Chatter.Application.Search;

public class PostSearchOptions : SearchOptions
{
	public Guid? UserId { get; set; }

	public ePostType? TypeId { get; set; }
}