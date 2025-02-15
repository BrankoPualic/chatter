using Chatter.Domain.Models;
using Chatter.Domain.Models.Application.Posts;

namespace Chatter.Application.Dtos.Comments;

public class CommentEditDto : IBaseDomain
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public string Content { get; set; }

	public Guid? ParentId { get; set; }

	public void ToModel(Comment model)
	{
		model.Id = Functions.AssignGuid(Id);
		model.UserId = UserId;
		model.PostId = PostId;
		model.Content = Content;
		model.ParentId = ParentId;
	}
}