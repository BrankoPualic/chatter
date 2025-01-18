namespace Chatter.Application.Dtos;

public class BlobDto
{
	public Guid Id { get; set; }

	public eBlobType TypeId { get; set; }

	public LookupValueDto Type { get; set; }

	public string MimeType { get; set; }

	public string Url { get; set; }

	public long Size { get; set; }

	public int? Duration { get; set; }
}