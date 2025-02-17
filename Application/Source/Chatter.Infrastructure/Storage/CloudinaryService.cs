using Chatter.Application.Dtos.Files;
using Chatter.Application.Interfaces;
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

	public async Task<DeletionResult> DeletePhotoAsync(string publicId)
	{
		var deletionParams = new DeletionParams(publicId);
		var result = await _cloudinary.DestroyAsync(deletionParams);
		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return result;
	}

	public async Task<DeletionResult> DeleteVideoAsync(string publicId)
	{
		var deletionParams = new DeletionParams(publicId) { ResourceType = ResourceType.Video };
		var result = await _cloudinary.DestroyAsync(deletionParams);
		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return result;
	}

	public async Task<ImageUploadResult> UploadPhotoAsync(FileInformationDto file, string directory)
	{
		if (file == null || file.Buffer == null || file.Size < 1)
			throw new Exception("No Image provided for upload");

		using var stream = new MemoryStream(file.Buffer);
		var uploadParams = new ImageUploadParams
		{
			File = new FileDescription(file.FileName, stream),
			Folder = $"{Common.Constants.CLOUDINARY_FILES_STORAGE}/{directory}",
			AllowedFormats = Common.Constants.ALLOWED_IMAGE_FORMATS,
		};

		var result = await _cloudinary.UploadAsync(uploadParams);

		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return result;
	}

	public async Task<VideoUploadResult> UploadVideoAsync(FileInformationDto file, string directory)
	{
		if (file == null || file.Buffer == null || file.Size < 1)
			throw new Exception("No Video provided for upload");

		using var stream = new MemoryStream(file.Buffer);
		var uploadParams = new VideoUploadParams
		{
			File = new FileDescription(file.FileName, stream),
			Folder = $"{Common.Constants.CLOUDINARY_VIDEO_STORAGE}/{directory}",
			AllowedFormats = Common.Constants.ALLOWED_VIDEO_FORMATS,
		};

		var result = await _cloudinary.UploadAsync(uploadParams);

		if (result.Error != null)
			throw new Exception(result.Error.Message);

		return result;
	}
}