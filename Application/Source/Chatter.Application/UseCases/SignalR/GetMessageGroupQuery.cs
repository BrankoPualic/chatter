using Chatter.Domain.Models.Application.SignalR;

namespace Chatter.Application.UseCases.SignalR;

public class GetMessageGroupQuery(Guid chatId) : BaseQuery<Group>
{
	public Guid ChatId { get; } = chatId;
}

internal class GetMessageGroupQueryHandler(IDatabaseContext db) : BaseQueryHandler<GetMessageGroupQuery, Group>(db)
{
	public override async Task<ResponseWrapper<Group>> Handle(GetMessageGroupQuery request, CancellationToken cancellationToken) =>
		new(await _db.Groups.GetSingleAsync(_ => _.ChatId == request.ChatId, [_ => _.Connections]));
}