using Chatter.Application.Dtos.Files;
using Chatter.Application.Dtos.Messaging;
using Chatter.Application.Interfaces;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Group;

public class SaveGroupCommand(GroupEditDto data, FileInformationDto file) : BaseCommand
{
	public GroupEditDto Data { get; } = data;

	public FileInformationDto File { get; } = file;
}

internal class SaveGroupCommandHandler(IDatabaseContext db, IBlobService blobService) : BaseCommandHandler<SaveGroupCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(SaveGroupCommand request, CancellationToken cancellationToken)
	{
		var model = await _db.Chats.GetSingleOrDefaultAsync(request.Data, [_ => _.Members]);

		request.Data.ToModel(model, _db);

		var blob = await blobService.UploadAsync(request.File);

		var oldBlob = model.GroupImageId;

		model.GroupImageId = blob?.Id;

		if (request.Data.Id == Guid.Empty)
			_db.Create(model);

		await _db.SaveChangesAsync(true, cancellationToken);

		if (oldBlob.HasValue)
			await blobService.DeleteAsync(blob.Id);

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