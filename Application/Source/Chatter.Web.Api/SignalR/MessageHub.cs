using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.Messaging.Messages;
using Chatter.Application.UseCases.SignalR;
using Chatter.Domain;
using Chatter.Domain.Models.Application.SignalR;
using Chatter.Web.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chatter.Web.Api.SignalR;

[Authorize]
public class MessageHub(IDatabaseContext db, IMediator mediator) : Hub
{
	public override async Task OnConnectedAsync()
	{
		var httpContext = Context.GetHttpContext();
		var chat = httpContext.Request.Query["chatId"];

		var isGuid = Guid.TryParse(chat, out Guid chatId);
		if (!isGuid)
			await Task.CompletedTask;

		await Groups.AddToGroupAsync(Context.ConnectionId, chat);

		await mediator.Send(new ReadUnreadMessagesCommand(chatId));

		// Add to group inside a database
		await AddToGroup(chatId);

		var messages = (await mediator.Send(new GetMessageListQuery(new Application.Search.MessageSearchOptions { ChatId = chatId }))).Data.Data;

		await Clients.Group(chat).SendAsync("ReceiveMessageThread", messages);
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		// Remove from database message group
		await RemoveFromMessageGroup();
		await base.OnDisconnectedAsync(exception);
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

	private async Task<bool> AddToGroup(Guid chatId)
	{
		var group = (await mediator.Send(new GetMessageGroupQuery(chatId))).Data;
		var connection = new Connection(Context.ConnectionId, Guid.Parse(Context.User.GetId()));

		if (group == null)
		{
			group = new(chatId);
			await mediator.Send(new CreateGroupCommand(group, false));
		}

		group.Connections.Add(connection);

		try
		{
			await db.SaveChangesAsync();
			return true;
		}
		catch
		{
			return false;
		}
	}

	private async Task RemoveFromMessageGroup()
	{
		var connection = (await mediator.Send(new GetConnectionQuery(Context.ConnectionId))).Data;
		await mediator.Send(new RemoveConnectionCommand(connection, false));
		await db.SaveChangesAsync();
	}
}