namespace Chatter.Application.Dtos.Messaging;

public class AttachmentDto
{
	public Guid Id { get; set; }

	public Guid MessageId { get; set; }

	public Guid? BlobId { get; set; }

	public int? Order { get; set; }

	public BlobDto Blob { get; set; }
}