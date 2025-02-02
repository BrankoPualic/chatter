using Chatter.Application.Dtos.Files;
using Chatter.Application.Interfaces;
using Chatter.Domain.Models.Application;

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
			TypeId = eBlobType.Image,
			Name = file.FileName,
			Type = file.Type,
			Size = file.Size,
			IsActive = true,
		};

		var result = await cloudinary.UploadAsync(file, blob.Id.ToString());

		blob.Url = result.SecureUrl.AbsoluteUri;
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

		await cloudinary.DeleteAsync(blob.PublicId);
	}
}