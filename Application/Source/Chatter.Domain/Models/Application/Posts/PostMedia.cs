using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Posts;

[PrimaryKey(nameof(PostId), nameof(BlobId))]
public class PostMedia
{
	public Guid PostId { get; set; }

	public Guid BlobId { get; set; }

	[ForeignKey(nameof(PostId))]
	public virtual Post Post { get; set; }

	[ForeignKey(nameof(BlobId))]
	public virtual Blob Blob { get; set; }
}
