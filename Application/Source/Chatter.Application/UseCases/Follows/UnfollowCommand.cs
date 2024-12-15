using Chatter.Application.Dtos.Follows;

namespace Chatter.Application.UseCases.Follows;

public class UnfollowCommand(FollowDto data) : BaseCommand
{
	public FollowDto Data { get; } = data;
}

internal class UnfollowCommandHandler(IDatabaseContext db) : BaseCommandHandler<UnfollowCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UnfollowCommand request, CancellationToken cancellationToken)
	{
		var follow = await _db.Follows
			.Where(_ => _.FollowingId == request.Data.FollowingId)
			.Where(_ => _.FollowerId == request.Data.FollowerId)
			.FirstOrDefaultAsync(cancellationToken);

		if (follow.IsNullOrEmpty())
			return new(ERROR_INVALID_OPERATION);

		_db.Follows.Remove(follow);
		await _db.SaveChangesAsync(false, cancellationToken);

		return new();
	}
}