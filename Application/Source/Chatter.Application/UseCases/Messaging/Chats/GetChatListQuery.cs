using Chatter.Application.Dtos.Messaging;
using Chatter.Application.Search;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Chats;

public class GetChatListQuery(ChatSearchOptions options) : BaseQuery<PagingResultDto<ChatDto>>
{
	public ChatSearchOptions Options { get; } = options;
}

internal class GetChatListQueryHandler(IDatabaseContext db, IIdentityUser currentUser) : BaseQueryHandler<GetChatListQuery, PagingResultDto<ChatDto>>(db, currentUser)
{
	public override async Task<ResponseWrapper<PagingResultDto<ChatDto>>> Handle(GetChatListQuery request, CancellationToken cancellationToken)
	{
		var filters = new List<Expression<Func<Chat, bool>>>()
		{
			_ => _.Members.Any(_ => _.UserId == _currentUser.Id)
		};

		if (request.Options.Filter.IsNotNullOrWhiteSpace())
		{
			filters.Add(_ => _.IsGroup
				? _.GroupName.Contains(request.Options.Filter)
				: _.Members.Any(_ => _.UserId != _currentUser.Id && _.User.Username.Contains(request.Options.Filter))
			);
		}

		var result = await _db.Chats.SearchAsync(request.Options, _ => _.LastMessageOn, true, filters);
		var ids = result.Data.SelectIds();

		var members = await _db.ChatMembers
			.Where(_ => ids.Contains(_.ChatId))
			.Select(_ => new
			{
				_.ChatId,
				_.UserId,
				_.User.Username,
				UserImage = _.User.Blobs
								.Where(_ => _.IsProfilePhoto == true && _.Blob.IsActive == true)
								.Select(_ => _.Blob)
								.FirstOrDefault(),
				_.IsMuted,
				_.User.GenderId
			})
			.ToListAsync(cancellationToken);

		var lastMessageStatuses = await _db.Chats
			.Where(_ => ids.Contains(_.Id))
			.Select(_ => new
			{
				ChatId = _.Id,
				Status = _.Messages
					.OrderByDescending(_ => _.CreatedOn)
					.Select(_ => _.Status)
					.FirstOrDefault()
			})
			.ToListAsync(cancellationToken);

		var data = new List<ChatDto>();

		foreach (var chat in result.Data)
		{
			var chatMembers = members.Where(_ => _.ChatId == chat.Id).ToList();

			var projection = new ChatDto()
			{
				Id = chat.Id,
				Name = chat.IsGroup ? chat.GroupName : chatMembers.Where(_ => _.UserId != _currentUser.Id).Select(_ => _.Username).FirstOrDefault(),
				IsMuted = chatMembers.Where(_ => _.UserId == _currentUser.Id).Select(_ => _.IsMuted).FirstOrDefault(),
				ImageUrl = chat.IsGroup ? null : chatMembers.Where(_ => _.UserId != _currentUser.Id).Select(_ => _.UserImage).FirstOrDefault()?.Url,
				LastMessageOn = chat.LastMessageOn,
				LastMessageStatusId = lastMessageStatuses.Where(_ => _.ChatId == chat.Id).Select(_ => _.Status).FirstOrDefault(),
				UserGenderId = chat.IsGroup ? null : chatMembers.Where(_ => _.UserId != _currentUser.Id).Select(_ => _.GenderId).FirstOrDefault(),
			};

			data.Add(projection);
		}

		data = data.OrderByDescending(_ => _.LastMessageOn)
		   .ThenBy(_ => _.Name)
		   .ToList();

		return new(new PagingResultDto<ChatDto>
		{
			Data = data,
			Total = result.Total
		});
	}
}