using Chatter.Application.Dtos.Messaging;
using Chatter.Application.Dtos.Users;

namespace Chatter.Application.UseCases.Messaging.Group;

public class GetGroupQuery(Guid chatId) : BaseQuery<GroupDto>
{
	public Guid ChatId { get; } = chatId;
}

internal class GetGroupQueryHandler(IDatabaseContext db) : BaseQueryHandler<GetGroupQuery, GroupDto>(db)
{
	public override async Task<ResponseWrapper<GroupDto>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
	{
		var group = await _db.Chats
			.Where(_ => _.Id == request.ChatId)
			.Select(_ => new { _.Id, _.GroupName, _.GroupImageId })
			.FirstOrDefaultAsync(cancellationToken);

		var photo = await _db.Blobs
			.Where(_ => _.Id == group.GroupImageId)
			.Select(_ => new { _.Id, _.Url })
			.FirstOrDefaultAsync(cancellationToken);

		var members = await _db.ChatMembers
			.Where(_ => _.ChatId == request.ChatId)
			.Select(_ => new UserLightDto
			{
				Id = _.UserId ?? Guid.Empty,
				Username = _.User.Username,
				ProfilePhoto = _.User.Blobs.Where(_ => _.TypeId == eUserMediaType.ProfilePhoto).Select(_ => _.Blob.Url).FirstOrDefault(),
				GenderId = _.User.GenderId,
				ChatRoleId = _.RoleId,
			})
			.ToListAsync(cancellationToken);

		return new(new GroupDto
		{
			ChatId = group.Id,
			GroupName = group.GroupName,
			GroupPhotoUrl = photo?.Url,
			BlobId = photo?.Id,
			Members = members
		});
	}
}