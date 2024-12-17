using Chatter.Application.Dtos.Users;
using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.Mapper;

public class UserMappingProfile : AutoMapperProfile
{
	public UserMappingProfile()
	{
		CreateMap<User, UserDto>()
		  .ForLookup(_ => _.Gender, _ => _.GenderId)
		  .ForMember(dest => dest.Followers, opt => opt.Ignore())
		  .ForMember(dest => dest.Following, opt => opt.Ignore());
		CreateMap<User, UserLightDto>();
	}
}