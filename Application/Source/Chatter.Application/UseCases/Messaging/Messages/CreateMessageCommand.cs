using Chatter.Application.Dtos.Messaging;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Messages;

public class CreateMessageCommand(MessageCreateDto data) : BaseCommand
{
	public MessageCreateDto Data { get; } = data;
}

internal class CreateMessageCommandHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseCommandHandler<CreateMessageCommand>(db, currentUser)
{
	public override async Task<ResponseWrapper> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
	{
		var now = DateTime.UtcNow;
		var chat = await _db.Chats
			.Where(_ => _.Members.Any(_ => _.UserId == _currentUser.Id) &&
				_.Members.Any(_ => _.UserId == request.Data.RecipientId)
			)
			.FirstOrDefaultAsync(cancellationToken);

		if (request.Data.ChatId.IsEmpty() && chat == null)
		{
			chat = new Chat()
			{
				LastMessageOn = now,
				Members = [
					new() { UserId = _currentUser.Id },
					new() { UserId = request.Data.RecipientId }
				]
			};

			_db.Create(chat);
		}
		else if (chat?.Id == request.Data.ChatId)
		{
			chat.LastMessageOn = now;
		}
		else
		{
			chat = await _db.Chats.GetSingleAsync(_ => _.Id == request.Data.ChatId);
			chat.LastMessageOn = now;
		}

		var model = new Message();
		request.Data.ToModel(model);
		model.ChatId = chat.Id;
		_db.Create(model);
		await _db.SaveChangesAsync(true, cancellationToken);
		return new();
	}
}