using Chatter.Application.Dtos.Messaging;
using Chatter.Application.Dtos.Users;
using Chatter.Application.Search;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Inbox;

public class GetChatQuery(MessageSearchOptions options) : BaseQuery<ChatLightDto>
{
	public MessageSearchOptions Options { get; } = options;
}

internal class GetChatQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : BaseQueryHandler<GetChatQuery, ChatLightDto>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<ChatLightDto>> Handle(GetChatQuery request, CancellationToken cancellationToken)
	{
		var filters = new List<Expression<Func<Message, bool>>>
		{
			_ => _.Chat.Members.Any(_ => _.UserId == _currentUser.Id)
		};

		if (request.Options.ChatId.HasValue)
			filters.Add(_ => _.ChatId == request.Options.ChatId.Value);

		var messages = await _db.Messages.SearchAsync(request.Options, _ => _.CreatedOn, false, filters,
			includeProperties: [
				_ => _.User,
				_ => _.Attachments.Select(_ => _.Blob)
		]);

		var chat = await _db.Chats
			.Include(_ => _.GroupImage)
			.Include(_ => _.Members)
			.ThenInclude(_ => _.User)
			.ThenInclude(_ => _.Blobs.Where(_ => _.TypeId == eUserMediaType.ProfilePhoto && _.Blob.IsActive == true))
			.ThenInclude(_ => _.Blob)
			.Where(_ => _.Id == request.Options.ChatId)
			.FirstOrDefaultAsync(cancellationToken);

		var result = new ChatLightDto
		{
			Id = chat?.Id,
			UserId = chat?.IsGroup == true ? null : chat?.Members.Where(_ => _.UserId != _currentUser.Id).FirstOrDefault().UserId,
			Name = chat?.IsGroup == true ? chat?.GroupName : chat?.Members.Where(_ => _.UserId != _currentUser.Id).FirstOrDefault().User.Username,
			IsGroup = chat?.IsGroup == true,
			IsMuted = chat?.Members.Where(_ => _.UserId == _currentUser.Id).FirstOrDefault().IsMuted == true,
			ImageUrl = chat?.IsGroup == true ? chat?.GroupImage?.Url : chat?.Members.Where(_ => _.UserId != _currentUser.Id).FirstOrDefault().User.Blobs.FirstOrDefault()?.Blob?.Url,
			UserGenderId = chat?.IsGroup == true ? null : chat?.Members.Where(_ => _.UserId != _currentUser.Id).FirstOrDefault().User.GenderId,
			//Messages = _mapper.To<PagingResultDto<MessageDto>>(messages),
			Messages = new PagingResultDto<MessageDto>
			{
				Total = messages?.Total ?? 0,
				Data = messages?.Data.Select(_ => new MessageDto
				{
					Id = _.Id,
					ChatId = _.ChatId,
					UserId = _.UserId,
					Content = _.Content,
					TypeId = _.TypeId,
					StatusId = _.Status,
					IsEditable = _.IsEditable(),
					IsMine = _.UserId == _currentUser.Id,
					CreatedOn = _.CreatedOn,
					User = _mapper.To<UserLightDto>(_.User),
					Attachments = _mapper.To<AttachmentDto>(_.Attachments).ToList()
				})
			}
		}; ;

		return new(result);
	}
}