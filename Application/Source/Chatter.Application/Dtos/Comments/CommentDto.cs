using Chatter.Application.Dtos.Users;

namespace Chatter.Application.Dtos.Comments;

public class CommentDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public string Content { get; set; }

	public Guid? ParentId { get; set; }

	public long? LikeCount { get; set; }

	public long? ReplyCount { get; set; }

	public UserLightDto User { get; set; }

	public List<CommentDto> Replies { get; set; } = [];
}