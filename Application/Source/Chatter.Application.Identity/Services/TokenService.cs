using Chatter.Application.Identity.Interfaces;
using Chatter.Common;
using Chatter.Domain.Models.Application.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chatter.Application.Identity.Services;

public class TokenService : ITokenService
{
	public string GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var jwtSecrets = new
		{
			Key = Encoding.UTF8.GetBytes(Settings.JwtKey),
			Settings.Issuer,
			Settings.Audience,
		};

		var claims = new List<Claim>
		{
			new(Constants.CLAIM_ID, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Iss, jwtSecrets.Issuer!),
			new(Constants.CLAIM_USERNAME, user.Username),
			new(Constants.CLAIM_EMAIL, user.Email)
		};

		var roles = user.Roles.Select(_ => ((int)_.RoleId).ToString()).ToList();

		claims.AddRange(roles.Select(role => new Claim(Constants.CLAIM_ROLES, role)));

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = jwtSecrets.Issuer,
			Audience = jwtSecrets.Audience,
			Expires = DateTime.UtcNow.AddDays(Constants.TOKEN_EXPIRATION_TIME),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecrets.Key), SecurityAlgorithms.HmacSha512Signature),
			Claims = claims.ToDictionary(claim => claim.Type, claim => (object)claim.Value)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}