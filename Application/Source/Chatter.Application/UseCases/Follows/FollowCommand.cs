using Chatter.Application.Dtos.Follows;
using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.UseCases.Follows;

public class FollowCommand(FollowDto data) : BaseCommand
{
	public FollowDto Data { get; } = data;
}

internal class FollowCommandHandler(IDatabaseContext db) : BaseCommandHandler<FollowCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(FollowCommand request, CancellationToken cancellationToken)
	{
		var ids = new List<Guid>() { request.Data.FollowingId, request.Data.FollowerId };
		var result = await _db.Users
			.Where(_ => ids.Contains(_.Id))
			.ToListAsync(cancellationToken);

		if (result.Count != 2)
			return new(ERROR_INVALID_OPERATION);

		var model = new UserFollow();
		request.Data.ToModel(model);

		_db.Follows.Add(model);
		await _db.SaveChangesAsync(false, cancellationToken);

		return new();
	}
}