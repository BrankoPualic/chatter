using Chatter.Application.Dtos.Users;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.Dtos.Messaging;

public class MessageDto
{
	public Guid Id { get; set; }

	public Guid ChatId { get; set; }

	public Guid? UserId { get; set; }

	public string Content { get; set; }

	public eMessageType TypeId { get; set; }

	public eMessageStatus StatusId { get; set; }

	public bool IsEditable { get; set; }

	public bool IsMine { get; set; }

	public DateTime CreatedOn { get; set; }

	public UserLightDto User { get; set; }

	public List<AttachmentDto> Attachments { get; set; } = [];

	public void ToModel(Message model)
	{
		model.ChatId = ChatId;
		model.UserId = UserId;
		model.Content = Content;
		model.TypeId = TypeId;
		model.Status = StatusId;
	}
}