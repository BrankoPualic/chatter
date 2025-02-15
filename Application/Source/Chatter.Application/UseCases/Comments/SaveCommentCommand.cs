using Chatter.Application.Dtos.Comments;

namespace Chatter.Application.UseCases.Comments;

public class SaveCommentCommand(CommentEditDto data) : BaseCommand
{
	public CommentEditDto Data { get; } = data;
}

internal class SaveCommentCommandHandler(IDatabaseContext db) : BaseCommandHandler<SaveCommentCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(SaveCommentCommand request, CancellationToken cancellationToken)
	{
		var model = await _db.Comments.GetSingleOrDefaultAsync(request.Data);

		request.Data.ToModel(model);

		if (request.Data.Id.IsEmpty())
			_db.Create(model);

		await _db.SaveChangesAsync(true, cancellationToken);

		return new();
	}
}

public class SaveCommentCommandValidator : AbstractValidator<SaveCommentCommand>
{
	public SaveCommentCommandValidator()
	{
		RuleFor(_ => _.Data.Content)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(CommentEditDto.Content)));
	}
}