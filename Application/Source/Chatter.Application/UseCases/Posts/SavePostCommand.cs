using Chatter.Application.Dtos.Files;
using Chatter.Application.Dtos.Posts;

namespace Chatter.Application.UseCases.Posts;

public class SavePostCommand(PostEditDto data, List<FileInformationDto> files) : BaseCommand
{
	public PostEditDto Data { get; } = data;

	public List<FileInformationDto> Files { get; } = files;
}

internal class SavePostCommandHandler(IDatabaseContext db, IBlobService blobService) : BaseCommandHandler<SavePostCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(SavePostCommand request, CancellationToken cancellationToken)
	{
		var model = await _db.Posts.GetSingleOrDefaultAsync(request.Data, [_ => _.Media.Select(_ => _.Blob)]);

		request.Data.ToModel(model);

		if (request.Files.IsNotNullOrEmpty())
		{
			foreach (var (file, index) in request.Files.WithIndex())
			{
				var blob = await blobService.UploadAsync(file);

				model.Media.Add(new()
				{
					PostId = model.Id,
					BlobId = blob.Id,
					Order = index + 1
				});
			}
		}

		if (request.Data.Id.IsEmpty())
			_db.Create(model);

		await _db.SaveChangesAsync(true, cancellationToken);

		return new();
	}
}