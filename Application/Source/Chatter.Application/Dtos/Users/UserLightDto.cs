namespace Chatter.Application.Dtos.Users;

public class UserLightDto
{
	public Guid Id { get; set; }

	public string Username { get; set; }

	public string ProfilePhoto { get; set; }

	public eGender? GenderId { get; set; }

	public bool IsFollowed { get; set; }
}