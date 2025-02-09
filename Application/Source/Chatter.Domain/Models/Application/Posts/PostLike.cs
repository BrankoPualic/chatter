using Chatter.Domain.Models.Application.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Posts;

[PrimaryKey(nameof(UserId), nameof(PostId))]
public class PostLike
{
	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public DateTime CreatedOn { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }

	[ForeignKey(nameof(PostId))]
	public virtual Post Post { get; set; }
}
