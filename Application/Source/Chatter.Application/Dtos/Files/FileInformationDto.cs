namespace Chatter.Application.Dtos.Files;

public class FileInformationDto
{
	public string FileName { get; set; }

	public string Type { get; set; }

	public long? Size { get; set; }

	public byte[] Buffer { get; set; }
}