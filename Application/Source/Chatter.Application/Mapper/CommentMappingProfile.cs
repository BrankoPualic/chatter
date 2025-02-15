using Chatter.Application.Dtos.Posts;
using Chatter.Domain.Models.Application.Posts;

namespace Chatter.Application.Mapper;

public class CommentMappingProfile : AutoMapperProfile
{
	public CommentMappingProfile()
	{
		CreateMap<Comment, CommentDto>();
	}
}