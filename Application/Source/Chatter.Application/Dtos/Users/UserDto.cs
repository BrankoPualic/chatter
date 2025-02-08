using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.Dtos.Users;

public class UserDto
{
	public Guid Id { get; set; }

	public string Username { get; set; }

	public string FullName { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string ProfilePhoto { get; set; }

	public string Thumbnail { get; set; }

	public eGender? GenderId { get; set; }

	public LookupValueDto Gender { get; set; }

	public bool IsPrivate { get; set; }

	public bool HasAccess { get; set; }

	public long Followers { get; set; }

	public long Following { get; set; }

	public Guid? ChatId { get; set; }

	public void ToModel(User model)
	{
		model.Username = Username;
		model.FirstName = FirstName;
		model.LastName = LastName;
		model.GenderId = GenderId;
		model.IsPrivate = IsPrivate;
	}
}