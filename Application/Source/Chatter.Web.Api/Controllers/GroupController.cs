using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.Messaging.Group;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Web.Api.Controllers;

public class GroupController(IMediator mediator) : BaseController(mediator)
{
	[HttpPost]
	[Authorize]
	[AngularMethod(typeof(void))]
	public async Task<IActionResult> CreateGroup(GroupCreateDto data) => Result(await Mediator.Send(new CreateGroupCommand(data)));
}