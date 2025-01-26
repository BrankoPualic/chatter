namespace Chatter.Application.UseCases.Messaging.Messages;

public class ReadUnreadMessagesCommand(Guid chatId) : BaseCommand
{
	public Guid ChatId { get; } = chatId;
}

internal class ReadUnreadMessagesCommandHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseCommandHandler<ReadUnreadMessagesCommand>(db, currentUser)
{
	public override async Task<ResponseWrapper> Handle(ReadUnreadMessagesCommand request, CancellationToken cancellationToken)
	{
		var messages = await _db.Messages
			.Where(_ => _.ChatId == request.ChatId)
			.Where(_ => _.UserId != _currentUser.Id)
			.Where(_ => _.Status == eMessageStatus.Delivered)
			.ToListAsync(cancellationToken);

		if (messages.Count == 0)
			return new();

		messages.ForEach(_ => _.Status = eMessageStatus.Seen);

		await _db.SaveChangesAsync(false, cancellationToken);

		return new();
	}
}