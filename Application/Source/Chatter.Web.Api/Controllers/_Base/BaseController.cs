using Chatter.Application;
using Chatter.Application.Dtos.Files;
using Chatter.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers._Base;

[Route("api/[controller]/[action]")]
[ApiController]
public class BaseController(IMediator mediator) : ControllerBase
{
	protected IMediator Mediator { get; } = mediator;

	protected async Task<FileInformationDto> GetFileAsync()
	{
		if (Request.Form.Files.IsNullOrEmpty())
			return null;

		var file = Request.Form.Files.FirstOrDefault();

		using var stream = new MemoryStream();
		await file.CopyToAsync(stream);

		return new()
		{
			FileName = file.FileName.Trim('\"'),
			Type = file.ContentType,
			Size = file.Length,
			Buffer = stream.ToArray()
		};
	}

	protected async Task<List<FileInformationDto>> GetFilesAsync()
	{
		var result = new List<FileInformationDto>();

		foreach (var file in Request.Form.Files.NotNull())
		{
			using var stream = new MemoryStream();
			await file.CopyToAsync(stream);

			result.Add(new()
			{
				FileName = file.FileName.Trim('\"'),
				Type = file.ContentType,
				Size = file.Length,
				Buffer = stream.ToArray()
			});
		}

		return result;
	}

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