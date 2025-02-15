namespace Chatter.Application.Search;

public class UserSearchOptions : SearchOptions
{
	public bool? IsFollowed { get; set; }

	public bool? IsNotSpokenTo { get; set; }

	public bool? IsNotPartOfGroup { get; set; }

	// ChatId
	public Guid? GroupId { get; set; }
}