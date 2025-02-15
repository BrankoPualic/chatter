using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Chatter.Web.Api;

public class ApplicationContractResolver : DefaultContractResolver
{
	private static CustomDefaultValueConverter CustomDefaultValueConverter { get; } = new CustomDefaultValueConverter();

	protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
	{
		var property = base.CreateProperty(member, memberSerialization);

		if (property.PropertyType == typeof(DateTime)
		|| property.PropertyType == typeof(DateTime?)
		|| property.PropertyType == typeof(bool)
		|| property.PropertyType == typeof(int)
		|| property.PropertyType == typeof(long)
		|| property.PropertyType == typeof(decimal)
		|| property.PropertyType.IsEnum
		)
		{
			property.Converter ??= CustomDefaultValueConverter;
		}

		var includeDefault = property.AttributeProvider.GetAttributes(typeof(IncludeDefaultAttribute), true).Any();
		if (includeDefault
			|| property.PropertyType == typeof(DateTime)
			|| property.PropertyType == typeof(int)
			|| property.PropertyType == typeof(long)
			|| property.PropertyType == typeof(decimal)
			|| property.PropertyType.IsEnum
		)
		{
			property.DefaultValueHandling = DefaultValueHandling.Include;
		}

		return property;
	}
}