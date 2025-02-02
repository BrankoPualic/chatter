using Chatter.Application.Dtos.Follows;

namespace Chatter.Application.UseCases.Follows;

public class IsFollowingQuery(FollowDto data) : BaseQuery<bool>
{
	public FollowDto Data { get; } = data;
}

internal class IsFollowingQueryHandler(IDatabaseContext db) : BaseQueryHandler<IsFollowingQuery, bool>(db)
{
	public override async Task<ResponseWrapper<bool>> Handle(IsFollowingQuery request, CancellationToken cancellationToken)
	{
		var follow = await _db.Follows
			.Where(_ => _.FollowingId == request.Data.FollowingId)
			.Where(_ => _.FollowerId == request.Data.FollowerId)
			.FirstOrDefaultAsync(cancellationToken);

		return new(follow != null);
	}
}