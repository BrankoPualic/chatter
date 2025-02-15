using System.Security.Claims;

namespace Chatter.Web.Api.Extensions;

public static class ClaimsExtensions
{
	public static Guid GetId(this IEnumerable<Claim> claims) => Guid.Parse(GetClaim(claims, Constants.CLAIM_ID));

	public static string GetEmail(this IEnumerable<Claim> claims) => GetClaim(claims, Constants.CLAIM_EMAIL);

	public static string GetUsername(this IEnumerable<Claim> claims) => GetClaim(claims, Constants.CLAIM_USERNAME);

	public static string GetRoles(this IEnumerable<Claim> claims) => GetClaim(claims, Constants.CLAIM_ROLES);

	public static string GetClaim(this IEnumerable<Claim> claims, string claimName) => claims.SingleOrDefault(i => i.Type.Equals(claimName)).Value;
}