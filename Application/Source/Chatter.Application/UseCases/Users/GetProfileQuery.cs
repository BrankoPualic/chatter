﻿using Chatter.Application.Dtos.Users;

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
			//.Include(_ => _.Blobs.Where(_ => (_.IsProfilePhoto == true || _.IsThumbnail == true) && _.Blob.IsActive == true))
			.Where(_ => _.Id == request.UserId)
			.FirstOrDefaultAsync(cancellationToken);
		if (result == null)
			return new(ERROR_NOT_FOUND);

		await _db.UserBlobs
			.Where(_ => _.UserId == request.UserId)
			.Where(_ => _.TypeId == eUserMediaType.ProfilePhoto || _.TypeId == eUserMediaType.Thumbnail)
			.Where(_ => _.Blob.IsActive == true)
			.Include(_ => _.Blob)
			.ToListAsync(cancellationToken);

		var follows = await _db.Follows
			.Where(_ => _.FollowerId == request.UserId || _.FollowingId == request.UserId)
			.GroupBy(_ => 1)
			.Select(_ => new
			{
				Following = _.Count(_ => _.FollowerId == request.UserId),
				Followers = _.Count(_ => _.FollowingId == request.UserId)
			})
			.FirstOrDefaultAsync(cancellationToken);

		var data = _mapper.To<UserDto>(result);

		data.ProfilePhoto = result.Blobs.Where(_ => _.TypeId == eUserMediaType.ProfilePhoto).Select(_ => _.Blob).FirstOrDefault()?.Url;
		data.Thumbnail = result.Blobs.Where(_ => _.TypeId == eUserMediaType.Thumbnail).Select(_ => _.Blob).FirstOrDefault()?.Url;

		data.Following = follows?.Following ?? 0;
		data.Followers = follows?.Followers ?? 0;

		data.ChatId = await _db.Chats
			.Where(_ => _.Members.Any(_ => _.UserId == request.UserId)
					&& _.Members.Any(_ => _.UserId == _currentUser.Id)
			)
			.Select(_ => _.Id)
			.FirstOrDefaultAsync(cancellationToken);

		data.HasAccess = _currentUser.Id == request.UserId
			|| !data.IsPrivate
			|| (await _db.Follows.FirstOrDefaultAsync(_ => _.FollowerId == _currentUser.Id && _.FollowingId == data.Id, cancellationToken) != null);

		return new(data);
	}
}