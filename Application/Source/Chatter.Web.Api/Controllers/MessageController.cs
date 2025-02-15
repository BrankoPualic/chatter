using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.Messaging.Messages;

namespace Chatter.Web.Api.Controllers;

public class MessageController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<MessageDto>))]
	public async Task<IActionResult> GetMessageList(MessageSearchOptions options) => Result(await Mediator.Send(new GetMessageListQuery(options)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(Guid))]
	public async Task<IActionResult> CreateMessage(MessageCreateDto data) => Result(await Mediator.Send(new CreateMessageCommand(data)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> EditMessage(MessageDto data) => Result(await Mediator.Send(new EditMessageCommand(data)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> DeleteMessage(Guid id) => Result(await Mediator.Send(new DeleteMessageCommand(id)));
}