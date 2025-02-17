using Chatter.Application.Dtos.Comments;
using Chatter.Application.UseCases.Comments;

namespace Chatter.Web.Api.Controllers;

public class CommentController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<CommentDto>))]
	public async Task<IActionResult> GetList(CommentSearchOptions options) => Result(await Mediator.Send(new GetCommentsQuery(options)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> Save(CommentEditDto data) => Result(await Mediator.Send(new SaveCommentCommand(data)));
}