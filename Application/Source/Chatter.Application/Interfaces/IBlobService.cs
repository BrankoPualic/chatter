using Chatter.Application.Dtos.Files;

namespace Chatter.Application.Interfaces;

public interface IBlobService
{
	Task<Blob> UploadAsync(FileInformationDto file);

	Task DeleteAsync(Guid blobId);
}