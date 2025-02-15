using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.Messaging.Inbox;

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