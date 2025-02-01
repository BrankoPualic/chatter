namespace Chatter.Application.Dtos.Messaging;

public class ChatDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public bool IsGroup { get; set; }

	public bool IsMuted { get; set; }

	public string ImageUrl { get; set; }

	public DateTime? LastMessageOn { get; set; }

	public string LastMessage { get; set; }

	public eMessageStatus? LastMessageStatusId { get; set; }

	public bool IsLastMessageMine { get; set; }

	public eGender? UserGenderId { get; set; }
}