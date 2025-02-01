using Chatter.Domain.Models.Application.SignalR;

namespace Chatter.Application.UseCases.SignalR;

public class RemoveConnectionCommand(Connection connection, bool saveChanges = true) : BaseCommand
{
	public Connection Connection { get; } = connection;

	public bool SaveChanges { get; } = saveChanges;
}

internal class RemoveConnectionCommandHandler(IDatabaseContext db) : BaseCommandHandler<RemoveConnectionCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(RemoveConnectionCommand request, CancellationToken cancellationToken)
	{
		_db.Remove(request.Connection);

		if (request.SaveChanges)
			await _db.SaveChangesAsync(false, cancellationToken);

		return new();
	}
}