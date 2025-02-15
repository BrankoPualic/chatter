using Chatter.Application.Dtos.Messaging;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.UseCases.Messaging.Messages;

public class GetMessageListQuery(MessageSearchOptions options) : BaseQuery<PagingResultDto<MessageDto>>
{
	public MessageSearchOptions Options { get; } = options;
}

internal class GeMessageListQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : BaseQueryHandler<GetMessageListQuery, PagingResultDto<MessageDto>>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<PagingResultDto<MessageDto>>> Handle(GetMessageListQuery request, CancellationToken cancellationToken)
	{
		var filters = new List<Expression<Func<Message, bool>>>
		{
			_ => _.Chat.Members.Any(_ => _.UserId == _currentUser.Id)
		};

		if (request.Options.ChatId.HasValue)
			filters.Add(_ => _.ChatId == request.Options.ChatId.Value);

		if (request.Options.RecipientId.HasValue)
			filters.Add(_ => _.Chat.Members.Any(_ => _.UserId == request.Options.RecipientId) && _.Chat.Members.Count() == 2);

		var result = await _db.Messages.SearchAsync(request.Options, _ => _.CreatedOn, false, filters,
			includeProperties: [
				_ => _.User,
				_ => _.Attachments.Select(_ => _.Blob)
		]);

		return new(_mapper.To<PagingResultDto<MessageDto>>(result));
	}
}