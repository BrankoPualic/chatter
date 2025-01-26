using Chatter.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chatter.Web.Api.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
	public override async Task OnConnectedAsync()
	{
		await tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);

		var currentUsers = await tracker.GetOnlineUsers();
		await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		await tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);

		var currentUsers = await tracker.GetOnlineUsers();

		await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

		await base.OnDisconnectedAsync(exception);
	}
}