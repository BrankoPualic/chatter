using System.ComponentModel;
using System.Reflection;

namespace Chatter.Common.Extensions;

public static class EnumExtensions
{
	public static List<T> GetEnumList<T>(this string enumsString) where T : struct, Enum
	{
		if (!enumsString.HasValue())
			return [];

		return enumsString.Split(',')
			.Select(e => Enum.TryParse<T>(e.Trim(), out var parsedEnum) ? parsedEnum : (T?)null)
			.Where(parsedEnum => parsedEnum.HasValue)
			.Select(parsedEnum => parsedEnum.Value)
			.ToList();
	}

	public static bool In<T>(this T value, params T[] args) where T : struct, Enum => args?.Contains(value) ?? false;

	public static string GetDescription(this Enum value)
	{
		FieldInfo field = value.GetType().GetField(value.ToString());

		return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute
			? value.ToString()
			: attribute.Description;
	}

	public static string[] GetEnumNames<T>(this IEnumerable<int> enums) where T : struct, Enum
		=> enums.Any() ? enums.Select(_ => ((T)(object)_).ToString()).ToArray() : [];

	public static List<T> GetEnums<T>() where T : struct, Enum
		=> !typeof(T).IsEnum
			? throw new ArgumentException($"{typeof(T)} must be an enumerated type.")
			: Enum.GetValues(typeof(T)).Cast<T>().ToList();
}