using Chatter.Application.Dtos.Users;
using Chatter.Application.Search;
using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.UseCases.Users;

public class GetUserListQuery(UserSearchOptions options) : BaseQuery<PagingResultDto<UserLightDto>>
{
	public UserSearchOptions Options { get; } = options;
}

internal class GetUserListQueryHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseQueryHandler<GetUserListQuery, PagingResultDto<UserLightDto>>(db, currentUser)
{
	public override async Task<ResponseWrapper<PagingResultDto<UserLightDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
	{
		if (request.Options.Filter.IsNullOrWhiteSpace())
			return new(new PagingResultDto<UserLightDto>());

		var filters = new List<Expression<Func<User, bool>>>()
		{
			_ => _.IsActive == true,
			_ => _.IsLocked == false,
			_ => _.Roles.Select(_ => _.RoleId).Contains(eSystemRole.Member)
		};

		if (request.Options.Filter.IsNotNullOrWhiteSpace())
			filters.Add(_ => _.Username.Contains(request.Options.Filter) || _.FullName.Contains(request.Options.Filter));

		var result = await _db.Users.SearchAsync(cancellationToken, request.Options, _ => _.Username, true, _ => new UserLightDto
		{
			Id = _.Id,
			Username = _.Username,
			GenderId = _.GenderId,
			ProfilePhoto = _.Blobs.Where(_ => _.IsProfilePhoto == true && _.IsActive == true).Any() ? _.Blobs.FirstOrDefault().Url : null,
			IsFollowed = _.Followers.Where(_ => _.FollowerId == _currentUser.Id).Any(),
		}, filters);

		return new(new PagingResultDto<UserLightDto>
		{
			Data = result.Data,
			Total = result.Total
		});
	}
}