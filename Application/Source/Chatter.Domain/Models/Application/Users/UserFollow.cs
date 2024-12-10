using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Users;

[PrimaryKey(nameof(FollowerId), nameof(FollowingId))]
public class UserFollow
{
	public Guid FollowerId { get; set; }

	public Guid FollowingId { get; set; }

	public DateTime FollowDate { get; set; }

	[ForeignKey(nameof(FollowerId))]
	public virtual User Follower { get; set; }

	[ForeignKey(nameof(FollowingId))]
	public virtual User Following { get; set; }
}