using Chatter.Application.Dtos.Messaging;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Group;

public class CreateGroupCommand(GroupCreateDto data) : BaseCommand
{
	public GroupCreateDto Data { get; } = data;
}

internal class CreateGroupCommandHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseCommandHandler<CreateGroupCommand>(db, currentUser)
{
	public override async Task<ResponseWrapper> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
	{
		var chat = new Chat
		{
			Id = Guid.NewGuid(),
			GroupName = request.Data.Name,
			IsGroup = true,
			Members = [
				new() { UserId = _currentUser.Id, RoleId = eChatRole.Admin }
			]
		};

		request.Data.Participants.ForEach(_ => chat.Members.Add(new() { UserId = _, RoleId = eChatRole.Member }));

		_db.Create(chat);

		await _db.SaveChangesAsync(true, cancellationToken);
		return new();
	}
}

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
	public CreateGroupCommandValidator()
	{
		RuleFor(_ => _.Data.Name)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(GroupCreateDto.Name)));
		RuleFor(_ => _.Data.Name)
			.MaximumLength(50).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(Chat.GroupName), 50))
			.When(_ => _.Data.Name.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.Participants)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(GroupCreateDto.Participants)));
	}
}