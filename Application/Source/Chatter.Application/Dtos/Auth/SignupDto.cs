using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.Dtos.Auth;

public class SignupDto
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Username { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public string ConfirmPassword { get; set; }

	public eGender GenderId { get; set; }

	public bool IsPrivate { get; set; }

	public void ToModel(User model)
	{
		model.FirstName = FirstName;
		model.LastName = LastName;
		model.Username = Username;
		model.Email = Email;
		model.GenderId = GenderId;
		model.IsPrivate = IsPrivate;
		model.IsActive = true;
		model.Roles = [new UserRole { RoleId = eSystemRole.Member }];
	}
}