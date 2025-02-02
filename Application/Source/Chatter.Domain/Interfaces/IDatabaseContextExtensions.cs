using Chatter.Common.Search;
using Chatter.Domain.Models;
using Chatter.Domain.Models.Application;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chatter.Domain.Interfaces;

public static class IDatabaseContextExtensions
{
	// Context Extensions
	public static void Create<TModel>(this IDatabaseContextBase db, TModel model) where TModel : class => db.Set<TModel>().Add(model);

	public static void DeleteSingle<TModel>(this IDatabaseContextBase db, Expression<Func<TModel, bool>> filter)
		where TModel : class
	{
		var entity = db.Set<TModel>().FirstOrDefault(filter)
			?? throw new ArgumentNullException(typeof(TModel).Name);

		db.Remove(entity);
	}

	public static void Remove<TModel>(this IDatabaseContextBase db, TModel model) where TModel : class => db.Set<TModel>().Remove(model);

	// DbSet Extensions

	public static async Task<TModel> GetSingleAsync<TModel>(this DbSet<TModel> dbSet, params object[] keyValue) where TModel : class => await dbSet.FindAsync(keyValue);

	public static async Task<TModel> GetSingleAsync<TModel>(this DbSet<TModel> dbSet, Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] includeProperties) where TModel : class
	{
		if (includeProperties == null)
			return await dbSet.FirstOrDefaultAsync(predicate);

		var query = includeProperties.Aggregate(dbSet.AsQueryable(), (current, property) => current.IncludeConvert(property));

		return await query.FirstOrDefaultAsync(predicate);
	}

	public static async Task<TModel> GetSingleOrDefaultAsync<TModel>(this DbSet<TModel> dbSet, IBaseDomain data, params Expression<Func<TModel, object>>[] includeProperties) where TModel : BaseDomain, new() =>
		data.Id == Guid.Empty
			? new()
			: await dbSet.GetSingleAsync(_ => _.Id == data.Id, includeProperties);

	public static async Task<PagingResult<TModel>> SearchAsync<TProperty, TModel>(this DbSet<TModel> dbSet, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, List<Expression<Func<TModel, bool>>> predicates, params Expression<Func<TModel, object>>[] includeProperties) where TModel : class =>
		await dbSet.SearchAsync(default, options, defaultOrder, desc, _ => _, predicates, includeProperties);

	public static async Task<PagingResult<TModel>> SearchAsync<TProperty, TModel>(this DbSet<TModel> dbSet, CancellationToken cancellationToken, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, List<Expression<Func<TModel, bool>>> predicates, params Expression<Func<TModel, object>>[] includeProperties) where TModel : class =>
		await dbSet.SearchAsync(cancellationToken, options, defaultOrder, desc, _ => _, predicates, includeProperties);

	public static async Task<PagingResult<TResult>> SearchAsync<TResult, TProperty, TModel>(this DbSet<TModel> dbSet, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, Expression<Func<TModel, TResult>> select, List<Expression<Func<TModel, bool>>> predicates, params Expression<Func<TModel, object>>[] includeProperties) where TModel : class =>
		await dbSet.SearchAsync(default, options, defaultOrder, desc, select, predicates, includeProperties);

	public static async Task<PagingResult<TResult>> SearchAsync<TResult, TProperty, TModel>(this DbSet<TModel> dbSet, CancellationToken cancellationToken, SearchOptions options, Expression<Func<TModel, TProperty>> defaultOrder, bool desc, Expression<Func<TModel, TResult>> select, List<Expression<Func<TModel, bool>>> predicates, params Expression<Func<TModel, object>>[] includeProperties) where TModel : class
	{
		var query = dbSet.AsNoTracking().AsQueryable();
		query = predicates.Aggregate(query, (current, expression) => current.Where(expression));

		var total = options.TotalCount == false ? 0 : await query.CountAsync(cancellationToken);

		if (includeProperties != null)
			query = includeProperties.Aggregate(query, (current, property) => current.IncludeConvert(property));

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

	// Private
	private static IQueryable<TModel> IncludeConvert<TModel>(this IQueryable<TModel> query, Expression<Func<TModel, object>> includeProperty) where TModel : class
	=> TryParsePath(includeProperty.Body, out var path)
		? query.Include(path)
		: throw new ArgumentException($"Invalid include parameter: {includeProperty}");

	private static bool TryParsePath(Expression expression, out string path)
	{
		path = null;
		var withoutConvert = expression.RemoveConvert();

		if (withoutConvert is MemberExpression memeberExpression)
		{
			var thisPart = memeberExpression.Member.Name;
			//if (!TryParsePath(memeberExpression.Expression, out var parentPart))
			//	return false;
			TryParsePath(memeberExpression.Expression, out var parentPart);
			path = parentPart == null ? thisPart : $"{parentPart}.{thisPart}";
			return true;
		}
		else if (withoutConvert is MethodCallExpression callExpression)
		{
			if (callExpression.Method.Name == "Select" && callExpression.Arguments.Count == 2)
			{
				if (!TryParsePath(callExpression.Arguments[0], out var parentPart))
					return false;

				if (parentPart != null)
					if (callExpression.Arguments[1] is LambdaExpression subExpression)
					{
						if (!TryParsePath(subExpression.Body, out var thisPart))
							return false;
						if (thisPart != null)
						{
							path = $"{parentPart}.{thisPart}";
							return true;
						}
					}
			}
			return false;
		}
		return false;
	}

	private static Expression RemoveConvert(this Expression expression)
	{
		while (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
			expression = ((UnaryExpression)expression).Operand;
		return expression;
	}
}