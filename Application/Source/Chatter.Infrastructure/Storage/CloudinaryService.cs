using Chatter.Common.Extensions;
using Chatter.Domain.Interfaces.Cloudinary;
using Chatter.Domain.Models.Cloudinary;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Chatter.Infrastructure.Storage;

public class CloudinaryService : ICloudinaryService
{
	private readonly Cloudinary _cloudinary;

	public CloudinaryService()
	{
		_cloudinary = new(
			new Account(
				Common.Settings.CloudName,
				Common.Settings.CloudStorageApiKey,
				Common.Settings.CloudStorageApiSecret
			)
		);
	}

	public async Task<CloudinaryDeletionResult> DeletePhotoAsync(string publicId)
	{
		var deletionParams = new DeletionParams(publicId);
		var result = await _cloudinary.DestroyAsync(deletionParams);
		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return new() { Result = result.Result };
	}

	public async Task<CloudinaryDeletionResult> DeleteVideoAsync(string publicId)
	{
		var deletionParams = new DeletionParams(publicId) { ResourceType = ResourceType.Video };
		var result = await _cloudinary.DestroyAsync(deletionParams);
		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return new() { Result = result.Result };
	}

	public async Task<CloudinaryUploadResult> UploadPhotoAsync(byte[] buffer, string fileName, string directory)
	{
		if (buffer == null || buffer.Length < 1 || fileName.IsNullOrWhiteSpace())
			throw new ArgumentException("No Image provided for upload");

		using var stream = new MemoryStream(buffer);
		var uploadParams = new ImageUploadParams
		{
			File = new FileDescription(fileName, stream),
			Folder = $"{Common.Constants.CLOUDINARY_FILES_STORAGE}/{directory}",
			AllowedFormats = Common.Constants.ALLOWED_IMAGE_FORMATS,
		};

		var result = await _cloudinary.UploadAsync(uploadParams);

		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return new() { Url = result.SecureUrl.AbsoluteUri, PublicId = result.PublicId };
	}

	public async Task<CloudinaryUploadResult> UploadVideoAsync(byte[] buffer, string fileName, string directory)
	{
		if (buffer == null || buffer.Length < 1 || fileName.IsNullOrWhiteSpace())
			throw new ArgumentException("No Video provided for upload");

		using var stream = new MemoryStream(buffer);
		var uploadParams = new VideoUploadParams
		{
			File = new FileDescription(fileName, stream),
			Folder = $"{Common.Constants.CLOUDINARY_VIDEO_STORAGE}/{directory}",
			AllowedFormats = Common.Constants.ALLOWED_VIDEO_FORMATS,
		};

		var result = await _cloudinary.UploadAsync(uploadParams);

		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return new() { Url = result.SecureUrl.AbsoluteUri, PublicId = result.PublicId };
	}
}