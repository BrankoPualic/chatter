using Chatter.Application.UseCases.Messaging.Chats;
using MediatR;

[TestFixture]
public partial class SandboxUT : BaseUT
{
	[Test, Explicit]
	public async Task SandboxBPR()
	{
		var response = await Get<IMediator>().Send(new GetChatListQuery(new Chatter.Application.Search.ChatSearchOptions()));
	}
}