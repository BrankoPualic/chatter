using Chatter.Application.UseCases.Auth;
using MediatR;

[TestFixture]
public partial class SandboxUT : BaseUT
{
	[Test, Explicit]
	public async Task SandboxBPR()
	{
		var result = await Get<IMediator>().Send(new LoginCommand(new() { Username = "admin", Password = "Pa$$w0rd" }));
	}
}