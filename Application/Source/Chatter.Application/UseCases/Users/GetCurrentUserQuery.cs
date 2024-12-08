using Chatter.Application.Dtos.Users;

namespace Chatter.Application.UseCases.Users;

public class GetCurrentUserQuery : BaseQuery<UserDto>
{
}

internal class GetCurrentUserQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : BaseQueryHandler<GetCurrentUserQuery, UserDto>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
	{
		var result = await _db.Users.FirstOrDefaultAsync(_ => _.Id == _currentUser.Id, cancellationToken);

		return result == null ? new(ERROR_NOT_FOUND) : new(_mapper.To<UserDto>(result));
	}
}