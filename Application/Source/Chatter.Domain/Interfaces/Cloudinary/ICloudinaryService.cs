using Chatter.Domain.Models.Cloudinary;

namespace Chatter.Domain.Interfaces.Cloudinary;

public interface ICloudinaryService
{
	Task<CloudinaryUploadResult> UploadPhotoAsync(byte[] buffer, string fileName, string directory);

	Task<CloudinaryUploadResult> UploadVideoAsync(byte[] buffer, string fileName, string directory);

	Task<CloudinaryDeletionResult> DeletePhotoAsync(string publicId);

	Task<CloudinaryDeletionResult> DeleteVideoAsync(string publicId);
}