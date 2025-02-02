using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Users;

[PrimaryKey(nameof(UserId), nameof(BlobId))]
public class UserBlob
{
	public Guid UserId { get; set; }

	public Guid BlobId { get; set; }

	public eUserMediaType TypeId { get; set; }

	[ForeignKey(nameof(UserId))]
	public User User { get; set; }

	[ForeignKey(nameof(BlobId))]
	public Blob Blob { get; set; }
}