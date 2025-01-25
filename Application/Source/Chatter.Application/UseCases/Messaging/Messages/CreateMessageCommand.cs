using Chatter.Application.Dtos.Messaging;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Messages;

public class CreateMessageCommand(MessageCreateDto data) : BaseCommand<Guid>
{
	public MessageCreateDto Data { get; } = data;
}

internal class CreateMessageCommandHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseCommandHandler<CreateMessageCommand, Guid>(db, currentUser)
{
	public override async Task<ResponseWrapper<Guid>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
	{
		var now = DateTime.UtcNow;
		var chat = await _db.Chats.GetSingleAsync(_ => _.Id == request.Data.ChatId);

		Guid? chatId = chat?.Id;
		if (request.Data.ChatId.IsEmpty() && chat == null)
		{
			chatId = Guid.NewGuid();
			chat = new Chat()
			{
				Id = (Guid)chatId,
				LastMessageOn = now,
				Members = [
					new() { UserId = _currentUser.Id },
					new() { UserId = request.Data.RecipientId }
				]
			};

			_db.Create(chat);
		}
		else
		{
			chat.LastMessageOn = now;
		}

		var model = new Message();
		request.Data.ToModel(model);
		model.ChatId = (Guid)chatId;
		_db.Create(model);
		await _db.SaveChangesAsync(true, cancellationToken);
		return new((Guid)chatId);
	}
}