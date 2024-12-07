using Chatter.Application.Identity.Interfaces;

namespace Chatter.Application.Identity.Services;

public class UserManager : IUserManager
{
	public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

	public bool VerifyPassword(string password, string storedPassword) => BCrypt.Net.BCrypt.Verify(password, storedPassword);
}