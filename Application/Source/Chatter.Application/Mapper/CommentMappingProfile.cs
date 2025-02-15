using Chatter.Application.Dtos.Comments;
using Chatter.Domain.Models.Application.Posts;

namespace Chatter.Application.Mapper;

public class CommentMappingProfile : AutoMapperProfile
{
	public CommentMappingProfile()
	{
		CreateMap<Comment, CommentDto>();
	}
}