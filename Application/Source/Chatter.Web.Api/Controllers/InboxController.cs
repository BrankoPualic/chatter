using Chatter.Application.Dtos;
using Chatter.Application.Dtos.Messaging;
using Chatter.Application.Search;
using Chatter.Application.UseCases.Messaging.Inbox;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class InboxController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<ChatDto>))]
	public async Task<IActionResult> GetInbox(InboxSearchOptions options) => Result(await Mediator.Send(new GetInboxQuery(options)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(ChatLightDto))]
	public async Task<IActionResult> GetChat(MessageSearchOptions options) => Result(await Mediator.Send(new GetChatQuery(options)));
}