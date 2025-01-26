using Chatter.Application.Search;
using Chatter.Domain.Interfaces;
using Chatter.Domain.Models.Application.Messaging;
using System.Linq.Expressions;

[TestFixture]
public partial class SandboxUT : BaseUT
{
	[Test, Explicit]
	public async Task SandboxBPR()
	{
		var options = new MessageSearchOptions
		{
			RecipientId = Guid.Parse("2D005C9A-FAE4-44F2-60A7-08DD1C7CC901")
		};
		var filters = new List<Expression<Func<Message, bool>>>
		{
			_ => _.Chat.Members.Any(_ => _.UserId == Guid.Parse("67AA6C84-71D4-4462-BE6B-08DD17CB6506"))
		};

		filters.Add(_ => _.Chat.Members.Any(_ => _.UserId == options.RecipientId) && _.Chat.Members.Count() == 2);

		var result = await db.Messages.SearchAsync(options, _ => _.CreatedOn, false, filters,
			includeProperties: [
				_ => _.User,
				_ => _.Attachments.Select(_ => _.Blob)
		]);
	}
}