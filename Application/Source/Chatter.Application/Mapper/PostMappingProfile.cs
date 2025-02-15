using Chatter.Application.Dtos.Posts;
using Chatter.Domain.Models.Application.Posts;

namespace Chatter.Application.Mapper;

public class PostMappingProfile : AutoMapperProfile
{
	public PostMappingProfile()
	{
		CreateMap<Post, PostDto>()
			.ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.Media.OrderBy(_ => _.Order).Select(_ => _.Blob)));
	}
}