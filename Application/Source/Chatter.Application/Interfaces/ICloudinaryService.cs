using Chatter.Application.Dtos.Files;
using CloudinaryDotNet.Actions;

namespace Chatter.Application.Interfaces;

public interface ICloudinaryService
{
	Task<ImageUploadResult> UploadAsync(FileInformationDto file, string directory);

	Task<DeletionResult> DeleteAsync(string publicId);
}