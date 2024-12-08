using Chatter.Application.Dtos.Users;
using Chatter.Application.UseCases.Users;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class UserController(IMediator mediator) : BaseController(mediator)
{
	[HttpGet]
	[AngularMethod(typeof(UserDto))]
	public async Task<IActionResult> GetCurrentUser() => Result(await Mediator.Send(new GetCurrentUserQuery()));
}