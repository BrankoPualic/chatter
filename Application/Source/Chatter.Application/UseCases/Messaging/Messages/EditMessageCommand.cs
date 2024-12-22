using Chatter.Application.Dtos.Messaging;

namespace Chatter.Application.UseCases.Messaging.Messages;

public class EditMessageCommand(MessageDto data) : BaseCommand
{
	public MessageDto Data { get; } = data;
}

internal class EditMessageCommandHandler(IDatabaseContext db) : BaseCommandHandler<EditMessageCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(EditMessageCommand request, CancellationToken cancellationToken)
	{
		var model = await _db.Messages.GetSingleAsync(_ => _.Id == request.Data.Id && _.IsEditable());
		if (model == null)
			return new(ERROR_INVALID_OPERATION);

		request.Data.ToModel(model);
		await db.SaveChangesAsync(true, cancellationToken);
		return new();
	}
}