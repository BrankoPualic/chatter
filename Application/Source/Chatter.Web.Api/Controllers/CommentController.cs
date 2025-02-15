using Chatter.Application.Dtos;
using Chatter.Application.Dtos.Comments;
using Chatter.Application.Search;
using Chatter.Application.UseCases.Comments;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class CommentController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<CommentDto>))]
	public async Task<IActionResult> GetComments(CommentSearchOptions options) => Result(await Mediator.Send(new GetCommentsQuery(options)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> SaveComment(CommentEditDto data) => Result(await Mediator.Send(new SaveCommentCommand(data)));
}