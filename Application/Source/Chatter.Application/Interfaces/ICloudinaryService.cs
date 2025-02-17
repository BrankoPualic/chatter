using Chatter.Application.Dtos.Files;
using CloudinaryDotNet.Actions;

namespace Chatter.Application.Interfaces;

public interface ICloudinaryService
{
	Task<DeletionResult> DeletePhotoAsync(string publicId);

	Task<DeletionResult> DeleteVideoAsync(string publicId);

	Task<ImageUploadResult> UploadPhotoAsync(FileInformationDto file, string directory);

	Task<VideoUploadResult> UploadVideoAsync(FileInformationDto file, string directory);
}