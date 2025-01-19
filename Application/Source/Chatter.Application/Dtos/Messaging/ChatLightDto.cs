namespace Chatter.Application.Dtos.Messaging;

public class ChatLightDto
{
	public Guid Id { get; set; }
	public Guid? UserId { get; set; }

	public string Name { get; set; }

	public bool IsGroup { get; set; }

	public bool IsMuted { get; set; }

	public string ImageUrl { get; set; }

	public eGender? UserGenderId { get; set; }

	public PagingResultDto<MessageDto> Messages { get; set; }
}