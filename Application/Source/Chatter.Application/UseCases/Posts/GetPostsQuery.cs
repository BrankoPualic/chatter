using Chatter.Application.Dtos.Posts;
using Chatter.Application.Search;
using Chatter.Domain.Models.Application.Posts;

namespace Chatter.Application.UseCases.Posts;

public class GetPostsQuery(PostSearchOptions options) : BaseQuery<PagingResultDto<PostDto>>
{
	public PostSearchOptions Options { get; } = options;
}

internal class GetPostsQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : BaseQueryHandler<GetPostsQuery, PagingResultDto<PostDto>>(db, currentUser, mapper)
{
	public override async Task<ResponseWrapper<PagingResultDto<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
	{
		var filters = new List<Expression<Func<Post, bool>>>();

		if (request.Options.UserId.HasValue)
			filters.Add(_ => _.UserId == request.Options.UserId.Value);
		if (request.Options.TypeId.HasValue)
			filters.Add(_ => _.TypeId == request.Options.TypeId.Value);

		var searchResult = await _db.Posts.SearchAsync(request.Options, _ => _.CreatedOn, true, filters,
			includeProperties: [
				_ => _.User,
				_ => _.Media.Select(_ => _.Blob)
			]);

		var postIds = searchResult.Data.SelectIds(_ => _.Id);

		var result = _mapper.To<PagingResultDto<PostDto>>(searchResult);

		var likesDictionary = await _db.PostLikes
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.Select(_ => new
			{
				PostId = _.Key,
				Likes = _.Count(),
			})
			.ToDictionaryAsync(_ => _.PostId, _ => _.Likes, cancellationToken: cancellationToken);

		var commentsDictionary = await _db.Comments
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.Select(_ => new
			{
				PostId = _.Key,
				Comments = _.Count(),
			})
			.ToDictionaryAsync(_ => _.PostId, _ => _.Comments, cancellationToken: cancellationToken);

		foreach (var post in result.Data)
		{
			post.LikeCount = likesDictionary.GetValue(post.Id);
			post.CommentCount = commentsDictionary.GetValue(post.Id);
		}

		return new(result);
	}
}