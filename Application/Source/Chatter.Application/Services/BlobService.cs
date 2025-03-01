using Chatter.Application.Dtos.Files;
using Chatter.Domain.Interfaces.Cloudinary;
using Chatter.Domain.Models.Cloudinary;

namespace Chatter.Application.Services;

public class BlobService(IDatabaseContext db, ICloudinaryService cloudinary) : IBlobService
{
	public async Task<Blob> UploadAsync(FileInformationDto file)
	{
		if (file == null)
			return null;

		var blob = new Blob
		{
			Id = Guid.NewGuid(),
			TypeId = GetBlobType(file),
			Name = file.FileName,
			Type = file.Type,
			Size = file.Size,
			IsActive = true,
		};

		var result = await UploadFileAsync(file, blob);

		blob.Url = result.Url;
		blob.PublicId = result.PublicId;

		db.Create(blob);

		return blob;
	}

	public async Task DeleteAsync(Guid blobId)
	{
		var blob = await db.Blobs.GetSingleAsync(blobId);
		if (blob == null)
			return;

		db.Remove(blob);
		await db.SaveChangesAsync();

		await DeleteFileAsync(blob);
	}

	// private

	private async Task<CloudinaryDeletionResult> DeleteFileAsync(Blob blob) => blob.TypeId switch
	{
		eBlobType.Image => await cloudinary.DeletePhotoAsync(blob.PublicId),
		eBlobType.Video => await cloudinary.DeleteVideoAsync(blob.PublicId),
		_ => throw new Exception("Invalid blob type")
	};

	private async Task<CloudinaryUploadResult> UploadFileAsync(FileInformationDto file, Blob blob) => blob.TypeId switch
	{
		eBlobType.Image => await cloudinary.UploadPhotoAsync(file.Buffer, file.FileName, blob.Id.ToString()),
		eBlobType.Video => await cloudinary.UploadVideoAsync(file.Buffer, file.FileName, blob.Id.ToString()),
		_ => throw new Exception("Invalid blob type")
	};

	private static eBlobType GetBlobType(FileInformationDto file) => file.Type switch
	{
		string type when type.StartsWith("video/") => eBlobType.Video,
		_ => eBlobType.Image
	};
}