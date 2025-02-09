using Chatter.Domain.Models.Application.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Posts;

public class Comment : BaseIndexAuditedDomain<Comment>
{
	public string Content { get; set; }

	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public Guid? ParentId { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }

	[ForeignKey(nameof(PostId))]
	public virtual Post Post { get; set; }

	[ForeignKey(nameof(ParentId))]
	public virtual Comment Parent { get; set; }

	[InverseProperty(nameof(Parent))]
	public virtual ICollection<Comment> Replies { get; set; } = [];
}