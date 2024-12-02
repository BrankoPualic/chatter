namespace Chatter.Application.Extensions;

public static class AutoMapperExtensions
{
	public static TDestination To<TDestination>(this IMapper mapper, object source) => mapper.Map<TDestination>(source);

	public static IEnumerable<TDestination> To<TDestination>(this IMapper mapper, IEnumerable<object> source) => mapper.Map<IEnumerable<TDestination>>(source);

	public static IMappingExpression<TSource, TDestination> ForLookup<TSource, TDestination, TEnum>(
			this IMappingExpression<TSource, TDestination> source,
			Expression<Func<TDestination, LookupValueDto>> lookupExpression,
			Func<TSource, TEnum?> sourceMappingExpression
		  )
		where TEnum : struct, Enum
			=> source.ForMember(lookupExpression, opt => opt.MapFrom((src, dest, prop, ctx) => sourceMappingExpression(src).ToLookupValueDto()));

	public static IMappingExpression<TSource, TDestination> ForLookup<TSource, TDestination, TEnum>(
		this IMappingExpression<TSource, TDestination> source,
		Expression<Func<TDestination, LookupValueDto>> lookupExpression,
		Func<TSource, TEnum> sourceMappingExpression
	  )
	where TEnum : struct, Enum
		=> source.ForMember(lookupExpression, opt => opt.MapFrom((src, dest, prop, ctx) => sourceMappingExpression(src).ToLookupValueDto()));
}