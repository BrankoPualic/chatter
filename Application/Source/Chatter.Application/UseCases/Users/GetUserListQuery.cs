using Chatter.Application.Dtos.Users;
using Chatter.Application.Search;

namespace Chatter.Application.UseCases.Users;

public class GetUserListQuery(UserSearchOptions options) : BaseQuery<PagingResult<UserLightDto>>
{
	public UserSearchOptions Options { get; } = options;
}

internal class GetUserListQueryHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseQueryHandler<GetUserListQuery, PagingResult<UserLightDto>>(db, currentUser)
{
	public override async Task<ResponseWrapper<PagingResult<UserLightDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
	{
		if (request.Options.Filter.IsNullOrWhiteSpace())
			return new(new PagingResult<UserLightDto>());

		var query = _db.Users
			.Where(_ => _.IsActive == true)
			.Where(_ => _.IsLocked == false)
			.Where(_ => _.Roles.Select(_ => _.RoleId).Contains(eSystemRole.Member))
			.AsNoTracking()
			.AsQueryable();

		if (request.Options.Filter.IsNotNullOrWhiteSpace())
			query = query.Where(_ => _.Username.Contains(request.Options.Filter) || _.FullName.Contains(request.Options.Filter));

		var total = request.Options.TotalCount == false ? 0 : await query.CountAsync(cancellationToken);

		if (request.Options.Take != 0)
			query = query.Skip(request.Options.Skip).Take(request.Options.Take);

		var result = new PagingResult<UserLightDto>
		{
			Total = total,
			Data = (await query
			.Select(_ => new UserLightDto
			{
				Id = _.Id,
				Username = _.Username,
				GenderId = _.GenderId,
				ProfilePhoto = _.Blobs.Where(_ => _.IsProfilePhoto == true && _.IsActive == true).Any() ? _.Blobs.FirstOrDefault().Url : null,
				IsFollowed = _.Followers.Where(_ => _.FollowerId == _currentUser.Id).Any(),
			})
			.ToListAsync(cancellationToken))
			// OFFLINE
			.OrderByDescending(_ => _.Username)
			.ToList()
		};

		return new(result);
	}
}