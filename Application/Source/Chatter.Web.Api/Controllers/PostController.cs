using Chatter.Application.Dtos;
using Chatter.Application.Dtos.Posts;
using Chatter.Application.Search;
using Chatter.Application.UseCases.Posts;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class PostController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<PostDto>))]
	public async Task<IActionResult> GetPosts(PostSearchOptions options) => Result(await Mediator.Send(new GetPostsQuery(options)));
}