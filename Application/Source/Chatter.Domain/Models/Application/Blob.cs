using Chatter.Domain.Interfaces;
using Chatter.Domain.Models.Application.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application;

public class Blob : BaseIndexAuditedDomain<Blob>
{
	public Guid UserId { get; set; }

	public eBlobType TypeId { get; set; }

	public string MimeType { get; set; }

	public string Url { get; set; }

	public string PublicId { get; set; }

	public bool? IsProfilePhoto { get; set; }

	public bool? IsThumbnail { get; set; }

	public long Size { get; set; }

	public int? Duration { get; set; }

	public bool? IsActive { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }

	//
	// Indexes
	//

	public static IDatabaseIndex IX_Blob_TypeId => new DatabaseIndex(nameof(IX_Blob_TypeId))
	{
		Columns = { nameof(TypeId) }
	};
}