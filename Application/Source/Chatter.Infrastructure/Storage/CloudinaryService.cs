﻿using Chatter.Application.Dtos.Files;
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

	public async Task<DeletionResult> DeleteAsync(string publicId) => await _cloudinary.DestroyAsync(new DeletionParams(publicId));

	public async Task<ImageUploadResult> UploadAsync(FileInformationDto file, string directory)
	{
		if (file == null || file.Buffer == null || file.Size < 1)
			return new();

		using var stream = new MemoryStream(file.Buffer);
		var uploadParams = new ImageUploadParams
		{
			File = new FileDescription(file.FileName, stream),
			Folder = $"{Common.Constants.CLOUDINARY_FILES_STORAGE}/{directory}",
		};

		return await _cloudinary.UploadAsync(uploadParams);
	}
}