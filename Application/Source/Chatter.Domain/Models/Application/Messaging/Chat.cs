using Chatter.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Messaging;

public class Chat : BaseIndexAuditedDomain<Chat>, IConfigurableEntity
{
	public string GroupName { get; set; }

	public bool IsGroup { get; set; }

	public DateTime LastMessageOn { get; set; }

	[InverseProperty(nameof(Chat))]
	public virtual ICollection<ChatMember> Members { get; set; } = [];

	[InverseProperty(nameof(Chat))]
	public virtual ICollection<Message> Messages { get; set; } = [];

	//
	// Indexes
	//

	public static IDatabaseIndex IX_Chat_IsGroup_LastMessageOn => new DatabaseIndex(nameof(IX_Chat_IsGroup_LastMessageOn))
	{
		Columns = { nameof(IsGroup), nameof(LastMessageOn) },
	};

	public void Configure(ModelBuilder builder)
	{
		builder.Entity<Chat>(_ =>
		{
			_.Property(_ => _.GroupName).HasMaxLength(50);
		});
	}
}