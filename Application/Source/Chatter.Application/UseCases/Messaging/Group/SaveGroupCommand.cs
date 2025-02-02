using Chatter.Application.Dtos.Files;
using Chatter.Application.Dtos.Messaging;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Group;

public class SaveGroupCommand(GroupEditDto data, FileInformationDto file) : BaseCommand
{
	public GroupEditDto Data { get; } = data;

	public FileInformationDto File { get; } = file;
}

internal class SaveGroupCommandHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseCommandHandler<SaveGroupCommand>(db, currentUser)
{
	public override async Task<ResponseWrapper> Handle(SaveGroupCommand request, CancellationToken cancellationToken)
	{
		var model = await _db.Chats.GetSingleOrDefaultAsync(request.Data, [_ => _.Members]);

		request.Data.ToModel(model, _db);

		if (request.Data.Id == Guid.Empty)
			_db.Create(model);

		await _db.SaveChangesAsync(true, cancellationToken);
		return new();
	}
}

public class CreateGroupCommandValidator : AbstractValidator<SaveGroupCommand>
{
	public CreateGroupCommandValidator()
	{
		RuleFor(_ => _.Data.Name)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(GroupEditDto.Name)));
		RuleFor(_ => _.Data.Name)
			.MaximumLength(50).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(Chat.GroupName), 50))
			.When(_ => _.Data.Name.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.Members)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(GroupEditDto.Members)));
	}
}