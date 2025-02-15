using Chatter.Application.Dtos.Comments;
using Chatter.Application.Search;
using Chatter.Domain.Models.Application.Posts;

namespace Chatter.Application.UseCases.Comments;

public class GetCommentsQuery(CommentSearchOptions options) : BaseQuery<PagingResultDto<CommentDto>>
{
	public CommentSearchOptions Options { get; } = options;
}

internal class GetCommentsQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : BaseQueryHandler<GetCommentsQuery, PagingResultDto<CommentDto>>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<PagingResultDto<CommentDto>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
	{
		var filters = new List<Expression<Func<Comment, bool>>>()
		{
			_ => _.PostId == request.Options.PostId
		};

		if (request.Options.ParentId.HasValue)
			filters.Add(_ => _.ParentId == request.Options.ParentId.Value);

		var searchResults = await _db.Comments.SearchAsync(request.Options, _ => _.CreatedBy, true, filters,
			includeProperties: [
				_ => _.User
			]);

		var commentIds = searchResults.Data.SelectIds(_ => _.Id);

		var result = _mapper.To<PagingResultDto<CommentDto>>(searchResults);

		var likesDictionary = await _db.CommentLikes
			.Where(_ => commentIds.Contains(_.CommentId))
			.GroupBy(_ => _.CommentId)
			.Select(_ => new
			{
				CommentId = _.Key,
				Likes = _.Count()
			})
			.ToDictionaryAsync(_ => _.CommentId, _ => _.Likes, cancellationToken: cancellationToken);

		var repliesDictionary = await _db.Comments
			.Where(_ => commentIds.Contains(_.Id))
			.GroupBy(_ => _.Id)
			.Select(_ => new
			{
				CommentId = _.Key,
				Replies = _.Count()
			})
			.ToDictionaryAsync(_ => _.CommentId, _ => _.Replies, cancellationToken: cancellationToken);

		foreach (var comment in result.Data)
		{
			comment.LikeCount = likesDictionary.GetValue(comment.Id);
			comment.ReplyCount = repliesDictionary.GetValue(comment.Id);
		}

		return new(result);
	}
}