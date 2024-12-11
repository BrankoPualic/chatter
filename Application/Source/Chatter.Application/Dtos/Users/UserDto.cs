namespace Chatter.Application.Dtos.Users;

public class UserDto
{
	public Guid Id { get; set; }

	public string Username { get; set; }

	public string FullName { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string ProfileImageUrl { get; set; }

	public eGender? GenderId { get; set; }

	public LookupValueDto Gender { get; set; }

	public bool IsPrivate { get; set; }

	public bool HasAccess { get; set; }

	public long Followers { get; set; }

	public long Following { get; set; }
}