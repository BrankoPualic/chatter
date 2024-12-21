using Chatter.Domain.Models.Application.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Messaging;

public class ChatMember : BaseIndexAuditedDomain<ChatMember>
{
	public Guid ChatId { get; set; }

	public Guid? UserId { get; set; }

	public eChatRole? RoleId { get; set; }

	public bool IsMuted { get; set; }

	public Guid? LastReadMessageId { get; set; }

	[ForeignKey(nameof(ChatId))]
	public virtual Chat Chat { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }

	[ForeignKey(nameof(LastReadMessageId))]
	public virtual Message LastReadMessage { get; set; }
}