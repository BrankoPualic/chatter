using Chatter.Domain.Models.Application.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Posts;

public class Post : BaseIndexAuditedDomain<Post>
{
	public string Content { get; set; }

	public Guid UserId { get; set; }

	public ePostType TypeId { get; set; }

	public bool IsCommentsDisabled { get; set; }

	public bool IsArchived { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }

	[InverseProperty(nameof(PostMedia.Post))]
	public virtual ICollection<PostMedia> Media { get; set; } = [];

	[InverseProperty(nameof(Comment.Post))]
	public virtual ICollection<Comment> Comments { get; set; } = [];
}