using Chatter.Application.UseCases.Users;
using MediatR;

[TestFixture]
public partial class SandboxUT : BaseUT
{
	[Test, Explicit]
	public async Task SandboxBPR()
	{
		var result = await Get<IMediator>().Send(new GetCurrentUserQuery());
	}
}