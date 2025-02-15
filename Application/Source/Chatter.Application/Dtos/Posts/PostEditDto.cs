using Chatter.Domain.Models;
using Chatter.Domain.Models.Application.Posts;

namespace Chatter.Application.Dtos.Posts;

public class PostEditDto : IBaseDomain
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public string Content { get; set; }

	public bool IsCommentsDisabled { get; set; }

	public ePostType TypeId { get; set; }

	public List<BlobDto> Media { get; set; } = [];

	public void ToModel(Post model)
	{
		model.Id = Functions.AssignGuid(Id);
		model.UserId = UserId;
		model.Content = Content;
		model.IsCommentsDisabled = IsCommentsDisabled;
		model.TypeId = TypeId;

		if (Media.Count > 0)
		{
			var missingMediaIds = model.Media
				.Where(_ => !Media.Any(m => m.Id == _.BlobId))
				.SelectIds(_ => _.BlobId);

			foreach (var media in model.Media)
			{
				if (missingMediaIds.Contains(media.BlobId))
					media.Blob.IsActive = false;
			}
		}
	}
}