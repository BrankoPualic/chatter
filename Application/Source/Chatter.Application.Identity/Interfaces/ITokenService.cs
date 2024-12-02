using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.Identity.Interfaces;

public interface ITokenService
{
	string GenerateJwtToken(User user);
}