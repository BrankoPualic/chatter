using Chatter.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chatter.Web.Api.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
	public override async Task OnConnectedAsync()
	{
		await tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
		await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

		var currentUsers = await tracker.GetOnlineUsers();
		await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		await tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);

		await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());

		var currentUsers = await tracker.GetOnlineUsers();

		await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

		await base.OnDisconnectedAsync(exception);
	}
}

public static class ClaimsPrincipalExtensions
{
	public static string GetUsername(this ClaimsPrincipal user)
	{
		return user.FindFirst(Constants.CLAIM_USERNAME)?.Value;
	}

	public static int GetUserId(this ClaimsPrincipal user)
	{
		return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
	}
}