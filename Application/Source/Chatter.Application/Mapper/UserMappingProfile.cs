using Chatter.Application.Dtos.Users;
using Chatter.Application.Extensions;
using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.Mapper;

public class UserMappingProfile : AutoMapperProfile
{
	public UserMappingProfile()
	{
		CreateMap<User, UserDto>()
		  .ForLookup(_ => _.Gender, _ => _.GenderId);
	}
}