using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Messaging;

public class Attachment : BaseIndexAuditedDomain<Attachment>
{
	public Guid MessageId { get; set; }

	public Guid? BlobId { get; set; }

	public int? Order { get; set; }

	[ForeignKey(nameof(MessageId))]
	public virtual Message Message { get; set; }

	[ForeignKey(nameof(BlobId))]
	public virtual Blob Blob { get; set; }
}