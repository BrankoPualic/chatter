using Chatter.Common.Search;

namespace Chatter.Application.Search;

public class UserSearchOptions : SearchOptions
{
	public bool? IsFollowed { get; set; }
}