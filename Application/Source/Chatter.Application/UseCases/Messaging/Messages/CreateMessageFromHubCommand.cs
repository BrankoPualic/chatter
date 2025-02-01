using Chatter.Application.Dtos.Messaging;
using Chatter.Application.UseCases.SignalR;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Messages;

public class CreateMessageFromHubCommand(MessageCreateDto data) : BaseCommand<MessageDto>
{
	public MessageCreateDto Data { get; } = data;
}

internal class CreateMessageFromHubCommandHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper, IMediator mediator) : BaseCommandHandler<CreateMessageFromHubCommand, MessageDto>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<MessageDto>> Handle(CreateMessageFromHubCommand request, CancellationToken cancellationToken)
	{
		var chat = await _db.Chats.GetSingleAsync(_ => _.Id == request.Data.ChatId, [_ => _.Members.Select(_ => _.User)]);
		chat.LastMessageOn = DateTime.UtcNow;

		var model = new Message();
		request.Data.ToModel(model);

		var group = (await mediator.Send(new GetMessageGroupQuery(chat.Id), cancellationToken)).Data;
		if (group.Connections.Any(_ => _.UserId == chat.Members.Where(_ => _.User.Id != _currentUser.Id).Select(_ => _.User.Id).FirstOrDefault()))
			model.Status = eMessageStatus.Seen;

		_db.Create(model);
		await _db.SaveChangesAsync(true, cancellationToken);
		model.User = chat.Members.Where(_ => _.UserId == _currentUser.Id).Select(_ => _.User).FirstOrDefault();
		return new(_mapper.To<MessageDto>(model));
	}
}