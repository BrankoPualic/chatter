using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.Messaging.Messages;
using Chatter.Web.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chatter.Web.Api.SignalR;

[Authorize]
public class MessageHub(IMediator mediator) : Hub
{
	public override async Task OnConnectedAsync()
	{
		var httpContext = Context.GetHttpContext();
		var otherUser = httpContext.Request.Query["userId"];
		var groupName = GetGroupName(Context.User.GetId(), otherUser);
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

		var isGuid = Guid.TryParse(otherUser, out Guid recipientId);
		if (!isGuid)
			await Task.CompletedTask;

		var messages = (await mediator.Send(new GetMessageListQuery(new Application.Search.MessageSearchOptions { RecipientId = recipientId }))).Data.Data;

		await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		return base.OnDisconnectedAsync(exception);
	}

	public async Task SendMessage(MessageCreateDto data)
	{
		var message = (await mediator.Send(new CreateMessageFromHubCommand(data))).Data;

		if (message != null)
		{
			var group = GetGroupName(data.SenderId.ToString(), data.RecipientId.ToString());
			await Clients.Group(group).SendAsync("NewMessage", message);
		}
	}

	public async Task StartTyping(Guid recipientId)
	{
		var group = GetGroupName(Context.User.GetId(), recipientId.ToString());
		await Clients.OthersInGroup(group).SendAsync("StartTyping");
	}

	public async Task StopTyping(Guid recipientId)
	{
		var group = GetGroupName(Context.User.GetId(), recipientId.ToString());
		await Clients.OthersInGroup(group).SendAsync("StopTyping");
	}

	private string GetGroupName(string caller, string other)
	{
		var stringCompare = string.CompareOrdinal(caller, other) < 0;
		return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
	}
}