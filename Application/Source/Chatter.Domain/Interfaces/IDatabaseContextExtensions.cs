using Chatter.Common.Search;
using Chatter.Domain.Models.Application;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chatter.Domain.Interfaces;

public static class IDatabaseContextExtensions
{
	public static async Task<PagingResult<TModel>> SearchAsync<TProperty, TModel>(this DbSet<TModel> dbSet, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, List<Expression<Func<TModel, bool>>> predicates) where TModel : class =>
		await dbSet.SearchAsync(default, options, defaultOrder, desc, _ => _, predicates);

	public static async Task<PagingResult<TModel>> SearchAsync<TProperty, TModel>(this DbSet<TModel> dbSet, CancellationToken cancellationToken, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, List<Expression<Func<TModel, bool>>> predicates) where TModel : class =>
		await dbSet.SearchAsync(cancellationToken, options, defaultOrder, desc, _ => _, predicates);

	public static async Task<PagingResult<TResult>> SearchAsync<TResult, TProperty, TModel>(this DbSet<TModel> dbSet, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, Expression<Func<TModel, TResult>> select, List<Expression<Func<TModel, bool>>> predicates) where TModel : class =>
		await dbSet.SearchAsync(default, options, defaultOrder, desc, select, predicates);

	public static async Task<PagingResult<TResult>> SearchAsync<TResult, TProperty, TModel>(this DbSet<TModel> dbSet, CancellationToken cancellationToken, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, Expression<Func<TModel, TResult>> select, List<Expression<Func<TModel, bool>>> predicates) where TModel : class
	{
		var query = dbSet.AsNoTracking().AsQueryable();
		query = predicates.Aggregate(query, (current, expression) => current.Where(expression));

		var total = options.TotalCount == false ? 0 : await query.CountAsync(cancellationToken);

		query = desc
			? query.OrderByDescending(defaultOrder)
			: query.OrderBy(defaultOrder);

		if (options.Take != 0)
			query = query.Skip(options.Skip).Take(options.Take);

		return new PagingResult<TResult>
		{
			Total = total,
			Data = await query.Select(select).ToListAsync(cancellationToken)
		};
	}
}