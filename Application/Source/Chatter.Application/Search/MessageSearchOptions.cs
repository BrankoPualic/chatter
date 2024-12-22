using Chatter.Common.Search;

namespace Chatter.Application.Search;

public class MessageSearchOptions : SearchOptions
{
	public Guid? ChatId { get; set; }
}