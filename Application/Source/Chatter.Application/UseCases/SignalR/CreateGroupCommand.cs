using Chatter.Domain.Models.Application.SignalR;

namespace Chatter.Application.UseCases.SignalR;

public class CreateGroupCommand(Group group, bool saveChanges = true) : BaseCommand
{
	public Group Group { get; } = group;

	public bool SaveChanges { get; } = saveChanges;
}

internal class CreateGroupCommandHandler(IDatabaseContext db) : BaseCommandHandler<CreateGroupCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
	{
		_db.Create(request.Group);

		if (request.SaveChanges)
			await _db.SaveChangesAsync(false, cancellationToken);

		return new();
	}
}