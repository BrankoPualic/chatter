using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Messages;

public class DeleteMessageCommand(Guid id) : BaseCommand
{
	public Guid Id { get; } = id;
}

internal class DeleteMessageCommandHandler(IDatabaseContext db) : BaseCommandHandler<DeleteMessageCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
	{
		_db.DeleteSingle<Message>(_ => _.Id == request.Id);
		await _db.SaveChangesAsync(false, cancellationToken);
		return new();
	}
}