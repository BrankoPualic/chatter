using Chatter.Application.Dtos;
using Chatter.Application.Dtos.Users;
using Chatter.Application.Search;
using Chatter.Application.UseCases.Users;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class UserController(IMediator mediator) : BaseController(mediator)
{
	[HttpGet]
	[Authorize]
	[AngularMethod(typeof(UserDto))]
	public async Task<IActionResult> GetCurrentUser() => Result(await Mediator.Send(new GetCurrentUserQuery()));

	[HttpGet("{userId}")]
	[Authorize]
	[AngularMethod(typeof(UserDto))]
	public async Task<IActionResult> GetProfile(Guid userId) => Result(await Mediator.Send(new GetProfileQuery(userId)));

	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(PagingResultDto<UserLightDto>))]
	public async Task<IActionResult> GetUserList(UserSearchOptions options) => Result(await Mediator.Send(new GetUserLightListQuery(options)));
}