using Chatter.Application.Dtos.Files;
using Chatter.Application.Dtos.Users;
using Chatter.Application.Interfaces;
using Chatter.Domain.Models.Application;
using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.UseCases.Users;

public class UpdateProfileCommand(UserDto data, List<FileInformationDto> files) : BaseCommand
{
	public UserDto Data { get; } = data;

	public List<FileInformationDto> Files { get; } = files;
}

internal class UpdateProfileCommandHandler(IDatabaseContext db, IBlobService blobService) : BaseCommandHandler<UpdateProfileCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
	{
		var model = await _db.Users.GetSingleAsync(_ => _.Id == request.Data.Id, [_ => _.Blobs.Select(_ => _.Blob)]);

		request.Data.ToModel(model);
		Blob oldThumbnail = null;

		if (request.Files.Count > 0)
		{
			foreach (var file in request.Files)
			{
				var isThumbnail = false;
				if (file.FileName == request.Data.Thumbnail)
					isThumbnail = true;

				var blob = await blobService.UploadAsync(file);

				var previousUserBlob = model.Blobs
					.Where(_ => _.TypeId == (isThumbnail ? eUserMediaType.Thumbnail : eUserMediaType.ProfilePhoto))
					.Where(_ => _.Blob.IsActive == true)
					.FirstOrDefault();

				if (isThumbnail && previousUserBlob != null)
				{
					oldThumbnail = previousUserBlob.Blob;
					_db.Remove(previousUserBlob);
				}
				else if (previousUserBlob != null)
					previousUserBlob.Blob.IsActive = false;

				var userBlob = new UserBlob
				{
					UserId = model.Id,
					BlobId = blob.Id,
					TypeId = isThumbnail ? eUserMediaType.Thumbnail : eUserMediaType.ProfilePhoto,
				};

				_db.Create(userBlob);
			}
		}

		await _db.SaveChangesAsync(true, cancellationToken);

		if (oldThumbnail != null)
			await blobService.DeleteAsync(oldThumbnail.Id);

		return new();
	}
}

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
	public UpdateProfileCommandValidator()
	{
		RuleFor(_ => _.Data.FirstName)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(UserDto.FirstName)));
		RuleFor(_ => _.Data.FirstName)
			.MinimumLength(3).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(UserDto.FirstName), 3))
			.MaximumLength(20).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(UserDto.FirstName), 20))
			.When(_ => _.Data.FirstName.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.LastName)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(UserDto.LastName)));
		RuleFor(_ => _.Data.LastName)
			.MinimumLength(3).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(UserDto.LastName), 3))
			.MaximumLength(20).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(UserDto.LastName), 30))
			.When(_ => _.Data.LastName.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.Username)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(UserDto.Username)));
		RuleFor(_ => _.Data.Username)
			.MinimumLength(3).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(UserDto.Username), 5))
			.MaximumLength(20).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(UserDto.Username), 20))
			.When(_ => _.Data.Username.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.GenderId)
			.NotEmpty()
			.Must(_ => _ > eGender.NotSet)
			.WithMessage(ResourcesValidation.Required.FormatWith("Gender"));
	}
}