using Chatter.Application.Dtos.Users;

namespace Chatter.Application.Dtos.Messaging;

public class GroupDto
{
	public Guid ChatId { get; set; }

	public string GroupName { get; set; }

	public string GroupPhotoUrl { get; set; }

	public Guid? BlobId { get; set; }

	public List<UserLightDto> Members { get; set; } = [];
}