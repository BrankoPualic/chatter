using Chatter.Common;
using Chatter.Domain.Interfaces;
using Chatter.Domain.Models.Application.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Messaging;

public class Message : BaseIndexAuditedDomain<Message>
{
	public Guid ChatId { get; set; }

	public Guid? UserId { get; set; }

	public string Content { get; set; }

	public eMessageType TypeId { get; set; }

	public eMessageStatus Status { get; set; }

	[ForeignKey(nameof(ChatId))]
	public virtual Chat Chat { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }

	[InverseProperty(nameof(Message))]
	public virtual ICollection<Attachment> Attachments { get; set; } = [];

	//
	//  Indexes
	//

	public static IDatabaseIndex IX_Message_TypeId => new DatabaseIndex(nameof(IX_Message_TypeId))
	{
		Columns = { nameof(TypeId) }
	};

	public static IDatabaseIndex IX_Message_ChatId_CreatedAt => new DatabaseIndex(nameof(IX_Message_ChatId_CreatedAt))
	{
		Columns = { nameof(ChatId), nameof(CreatedOn) }
	};

	// Predicates
	public bool IsEditable() => (DateTime.UtcNow - CreatedOn).TotalSeconds <= Constants.MESSAGE_EDIT_TIME_LIMIT_SECONDS;
}