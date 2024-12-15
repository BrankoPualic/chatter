using Chatter.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers._Base;

[Route("api/[controller]/[action]")]
[ApiController]
public class BaseController(IMediator mediator) : ControllerBase
{
	protected IMediator Mediator { get; } = mediator;

	public IActionResult Result(ResponseWrapper response) => response.IsSuccess ? Ok() : BadRequest(response.Errors);

	public IActionResult Result(ResponseWrapper response, string message) => response.IsSuccess ? Ok(message) : BadRequest(response.Errors);

	public IActionResult Result<T>(ResponseWrapper<T> response) => response.IsSuccess ? Ok(response.Data) : BadRequest(response.Errors);

	public IActionResult ResultCreated(ResponseWrapper response) => response.IsSuccess ? Created() : BadRequest(response.Errors);

	public IActionResult ResultNoContent(ResponseWrapper response) => response.IsSuccess ? NoContent() : BadRequest(response.Errors);

	public IActionResult ResultNotFound(ResponseWrapper response) => response.IsSuccess ? Ok() : NotFound(response.Errors);

	public IActionResult ResultNotFound<T>(ResponseWrapper<T> response) => response.IsSuccess ? Ok(response.Data) : NotFound(response.Errors);

	public IActionResult ResultWrapper<T>(Func<T> func) => Result(ResponseWrapper.TryRun(func));

	public async Task<IActionResult> ResultWrapper(Task func) => Result(await ResponseWrapper.TryRunAsync(func));

	public async Task<IActionResult> ResultWrapper<T>(Task<T> func) => Result(await ResponseWrapper.TryRunAsync(func));
}