using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.Dtos.Follows;

public class FollowDto
{
	public Guid FollowerId { get; set; }

	public Guid FollowingId { get; set; }

	public void ToModel(UserFollow model)
	{
		model.FollowerId = FollowerId;
		model.FollowingId = FollowingId;
		model.FollowDate = DateTime.UtcNow;
	}
}