using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.Dtos.Messaging;

public class MessageCreateDto
{
	public Guid ChatId { get; set; }

	public Guid SenderId { get; set; }

	public Guid RecipientId { get; set; }

	public string Content { get; set; }

	public eMessageType TypeId { get; set; }

	public eMessageStatus StatusId { get; set; }

	public List<AttachmentDto> Attachments { get; set; } = [];

	public void ToModel(Message model)
	{
		model.ChatId = ChatId;
		model.UserId = SenderId;
		model.Content = Content;
		model.TypeId = TypeId;
		model.Status = eMessageStatus.Delivered;
	}
}