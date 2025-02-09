using Chatter.Domain.Models.Application.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Posts;

[PrimaryKey(nameof(UserId), nameof(CommentId))]
public class CommentLike
{
	public Guid UserId { get; set; }

	public Guid CommentId { get; set; }

	public DateTime CreatedOn { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }

	[ForeignKey(nameof(CommentId))]
	public virtual Comment Comment { get; set; }
}