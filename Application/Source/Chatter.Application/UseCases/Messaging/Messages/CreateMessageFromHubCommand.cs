﻿using Chatter.Application.Dtos.Messaging;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Messages;

public class CreateMessageFromHubCommand(MessageCreateDto data) : BaseCommand<MessageDto>
{
	public MessageCreateDto Data { get; } = data;
}

internal class CreateMessageFromHubCommandHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : BaseCommandHandler<CreateMessageFromHubCommand, MessageDto>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<MessageDto>> Handle(CreateMessageFromHubCommand request, CancellationToken cancellationToken)
	{
		var chat = await _db.Chats.GetSingleAsync(_ => _.Id == request.Data.ChatId);
		chat.LastMessageOn = DateTime.UtcNow;

		var model = new Message();
		request.Data.ToModel(model);
		_db.Create(model);
		await _db.SaveChangesAsync(true, cancellationToken);
		return new(_mapper.To<MessageDto>(model));
	}
}