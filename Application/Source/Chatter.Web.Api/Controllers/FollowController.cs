﻿using Chatter.Application.Dtos.Follows;
using Chatter.Application.UseCases.Follows;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class FollowController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(bool))]
	public async Task<IActionResult> IsFollowing(FollowDto data) => Result(await Mediator.Send(new IsFollowingQuery(data)));

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Follow(FollowDto data) => Result(await Mediator.Send(new FollowCommand(data)));

	[HttpPost]
	public async Task<IActionResult> Unfollow(FollowDto data) => Result(await Mediator.Send(new UnfollowCommand(data)));
}