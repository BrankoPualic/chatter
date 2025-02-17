using Chatter.Application.Dtos.Posts;
using Chatter.Application.UseCases.Posts;

namespace Chatter.Web.Api.Controllers;

public class PostController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<PostDto>))]
	public async Task<IActionResult> GetList(PostSearchOptions options) => Result(await Mediator.Send(new GetPostsQuery(options)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> Save([ModelBinder(BinderType = typeof(JsonModelBinder))] PostEditDto model) => Result(await Mediator.Send(new SavePostCommand(model, await GetFilesAsync())));
}