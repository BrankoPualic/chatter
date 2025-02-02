using Chatter.Application.Dtos.Files;
using Chatter.Domain.Models.Application;

namespace Chatter.Application.Interfaces;

public interface IBlobService
{
	Task<Blob> UploadAsync(FileInformationDto file);

	Task DeleteAsync(Guid blobId);
}