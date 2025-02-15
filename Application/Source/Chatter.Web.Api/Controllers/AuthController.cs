using Chatter.Application.Dtos.Auth;
using Chatter.Application.UseCases.Auth;

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