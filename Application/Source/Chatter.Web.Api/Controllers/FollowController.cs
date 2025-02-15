using Chatter.Application.Dtos.Follows;
using Chatter.Application.UseCases.Follows;

namespace Chatter.Web.Api.Controllers;

public class FollowController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(bool))]
	public async Task<IActionResult> IsFollowing(FollowDto data) => Result(await Mediator.Send(new IsFollowingQuery(data)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> Follow(FollowDto data) => Result(await Mediator.Send(new FollowCommand(data)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> Unfollow(FollowDto data) => Result(await Mediator.Send(new UnfollowCommand(data)));
}