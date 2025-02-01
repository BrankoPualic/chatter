namespace Chatter.Application.Dtos.Messaging;

public class GroupCreateDto
{
	public string Name { get; set; }

	public List<Guid> Participants { get; set; } = [];
}