using Chatter.Domain.Models.Application.SignalR;

namespace Chatter.Application.UseCases.SignalR;

public class GetConnectionQuery(string connectionId) : BaseQuery<Connection>
{
	public string ConnectionId { get; } = connectionId;
}

internal class GetConnectionQueryHandler(IDatabaseContext db) : BaseQueryHandler<GetConnectionQuery, Connection>(db)
{
	public override async Task<ResponseWrapper<Connection>> Handle(GetConnectionQuery request, CancellationToken cancellationToken) =>
		new(await _db.Connections.GetSingleAsync(request.ConnectionId, cancellationToken));
}