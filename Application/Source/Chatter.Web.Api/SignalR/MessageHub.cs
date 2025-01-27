using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.Messaging.Messages;
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
		var chat = httpContext.Request.Query["chatId"];
		await Groups.AddToGroupAsync(Context.ConnectionId, chat);

		var isGuid = Guid.TryParse(chat, out Guid chatId);
		if (!isGuid)
			await Task.CompletedTask;

		var messages = (await mediator.Send(new GetMessageListQuery(new Application.Search.MessageSearchOptions { ChatId = chatId }))).Data.Data;

		await Clients.Group(chat).SendAsync("ReceiveMessageThread", messages);
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
			var group = data.ChatId.ToString();
			await Clients.Group(group).SendAsync("NewMessage", message);
		}
	}

	public async Task StartTyping(Guid chatId)
	{
		var group = chatId.ToString();
		await Clients.OthersInGroup(group).SendAsync("StartTyping");
	}

	public async Task StopTyping(Guid chatId)
	{
		var group = chatId.ToString();
		await Clients.OthersInGroup(group).SendAsync("StopTyping");
	}
}