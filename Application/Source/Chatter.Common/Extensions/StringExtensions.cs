namespace Chatter.Common.Extensions;

public static class StringExtensions
{
	public static bool IsNullOrEmpty(this string text) => string.IsNullOrEmpty(text);

	public static bool IsNullOrWhiteSpace(this string text) => string.IsNullOrWhiteSpace(text);

	public static bool IsNotNullOrEmpty(this string text) => !string.IsNullOrEmpty(text);

	public static bool IsNotNullOrWhiteSpace(this string text) => !string.IsNullOrWhiteSpace(text);

	public static string IfNotNullOrWhiteSpace(this string text, string? format = null, string defaultValue = "") => text.IsNotNullOrWhiteSpace() ? (format.IsNotNullOrEmpty() ? string.Format(format, text) : text) : defaultValue;

	public static string IfNotNullOrEmpty(this string text, string format = null, string defaultValue = "") => text.IsNotNullOrEmpty() ? (format.IsNotNullOrEmpty() ? string.Format(format, text) : text) : defaultValue;

	public static string Join(this IEnumerable<string> source, string spearator, bool removeNullOrWhiteSpace = true) => removeNullOrWhiteSpace ? string.Join(spearator, source.Where(_ => _.IsNotNullOrWhiteSpace())) : string.Join(spearator, source);

	public static bool HasValue(this string text) => text.IsNotNullOrEmpty() && text.IsNotNullOrWhiteSpace();

	public static bool In(this string text, params string[] args) => args.Contains(text);
}