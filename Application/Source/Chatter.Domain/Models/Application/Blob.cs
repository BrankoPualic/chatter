using Chatter.Domain.Interfaces;

namespace Chatter.Domain.Models.Application;

public class Blob : BaseIndexAuditedDomain<Blob>
{
	public eBlobType TypeId { get; set; }

	public string Type { get; set; }

	public string Url { get; set; }

	public string PublicId { get; set; }

	public long? Size { get; set; }

	public bool? IsActive { get; set; }

	//
	// Indexes
	//

	public static IDatabaseIndex IX_Blob_TypeId => new DatabaseIndex(nameof(IX_Blob_TypeId))
	{
		Columns = { nameof(TypeId) }
	};
}