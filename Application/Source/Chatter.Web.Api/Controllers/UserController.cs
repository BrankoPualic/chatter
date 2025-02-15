using Chatter.Application.Dtos.Users;
using Chatter.Application.UseCases.Users;

namespace Chatter.Web.Api.Controllers;

public class UserController(IMediator mediator) : BaseController(mediator)
{
	[HttpGet]
	[Authorize]
	[AngularMethod(typeof(UserDto))]
	public async Task<IActionResult> GetCurrentUser() => Result(await Mediator.Send(new GetCurrentUserQuery()));

	[HttpGet]
	[Authorize]
	[AngularMethod(typeof(UserDto))]
	public async Task<IActionResult> GetProfile([FromQuery] Guid userId) => Result(await Mediator.Send(new GetProfileQuery(userId)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<UserLightDto>))]
	public async Task<IActionResult> GetUserList(UserSearchOptions options) => Result(await Mediator.Send(new GetUserLightListQuery(options)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> UpdateProfile([ModelBinder(BinderType = typeof(JsonModelBinder))] UserDto model) => Result(await Mediator.Send(new UpdateProfileCommand(model, await GetFilesAsync())));
}