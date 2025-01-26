using Chatter.Common;
using System.Security.Claims;

namespace Chatter.Web.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static string GetUsername(this ClaimsPrincipal user) => user.FindFirst(Constants.CLAIM_USERNAME)?.Value;

	public static string GetId(this ClaimsPrincipal user) => user.FindFirst(Constants.CLAIM_ID)?.Value;
}