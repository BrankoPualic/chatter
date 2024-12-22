using Chatter.Application.Dtos;
using Chatter.Application.Dtos.Messaging;
using Chatter.Application.Search;
using Chatter.Application.UseCases.Messaging.Chats;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class ChatController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<ChatDto>))]
	public async Task<IActionResult> GetChatList(ChatSearchOptions options) => Result(await Mediator.Send(new GetChatListQuery(options)));
}