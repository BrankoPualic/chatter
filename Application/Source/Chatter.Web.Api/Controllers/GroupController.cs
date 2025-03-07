﻿using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.Messaging.Group;

namespace Chatter.Web.Api.Controllers;

public class GroupController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> Save([ModelBinder(BinderType = typeof(JsonModelBinder))] GroupEditDto model) => Result(await Mediator.Send(new SaveGroupCommand(model, await GetFileAsync())));

	[HttpGet]
	[Authorize]
	[AngularMethod(typeof(GroupDto))]
	public async Task<IActionResult> GetSingle([FromQuery] Guid chatId) => Result(await Mediator.Send(new GetGroupQuery(chatId)));
}