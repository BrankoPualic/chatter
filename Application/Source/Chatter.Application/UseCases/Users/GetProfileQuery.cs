using Chatter.Application.Dtos.Users;

namespace Chatter.Application.UseCases.Users;

public class GetProfileQuery(Guid userId) : BaseQuery<UserDto>
{
	public Guid UserId { get; } = userId;
}

internal class GetProfileQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : BaseQueryHandler<GetProfileQuery, UserDto>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<UserDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
	{
		var result = await _db.Users
			.Include(_ => _.Blobs.Where(_ => (_.IsProfilePhoto == true || _.IsThumbnail == true) && _.IsActive == true))
			.Where(_ => _.Id == request.UserId)
			.FirstOrDefaultAsync(cancellationToken);
		if (result == null)
			return new(ERROR_NOT_FOUND);

		var follows = await _db.Follows
			.Where(_ => _.FollowerId == request.UserId || _.FollowingId == request.UserId)
			.GroupBy(_ => _.FollowingId == request.UserId)
			.Select(_ => new
			{
				Following = _.Count(_ => _.FollowerId == request.UserId),
				Followers = _.Count(_ => _.FollowingId == request.UserId)
			})
			.FirstOrDefaultAsync(cancellationToken);

		var data = _mapper.To<UserDto>(result);

		data.ProfilePhoto = result.Blobs.Where(_ => _.IsProfilePhoto == true).FirstOrDefault()?.Url;
		data.Thumbnail = result.Blobs.Where(_ => _.IsThumbnail == true).FirstOrDefault()?.Url;

		data.Following = follows?.Following ?? 0;
		data.Followers = follows?.Followers ?? 0;

		data.HasAccess = _currentUser.Id == request.UserId
			|| !data.IsPrivate
			|| (await _db.Follows.FirstOrDefaultAsync(_ => _.FollowerId == _currentUser.Id && _.FollowingId == data.Id, cancellationToken) != null);

		return new(data);
	}
}