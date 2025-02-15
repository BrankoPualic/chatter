using Chatter.Application.Dtos.Comments;
using Chatter.Application.Dtos.Users;

namespace Chatter.Application.Dtos.Posts;

public class PostDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public string Content { get; set; }

	public ePostType TypeId { get; set; }

	public bool IsCommentsDisabled { get; set; }

	public long? LikeCount { get; set; }

	public long? CommentCount { get; set; }

	public UserLightDto User { get; set; }

	public List<BlobDto> Media { get; set; } = [];

	public List<CommentDto> Comments { get; set; } = [];
}