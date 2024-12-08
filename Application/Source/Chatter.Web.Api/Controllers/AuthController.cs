using Chatter.Application.Dtos.Auth;
using Chatter.Application.UseCases.Auth;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class AuthController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[AngularMethod(typeof(TokenDto))]
	public async Task<IActionResult> Login(LoginDto data) => Result(await Mediator.Send(new LoginCommand(data)));

	[HttpPost]
	[AngularMethod(typeof(TokenDto))]
	public async Task<IActionResult> Signup(SignupDto data) => Result(await Mediator.Send(new SignupCommand(data)));
}