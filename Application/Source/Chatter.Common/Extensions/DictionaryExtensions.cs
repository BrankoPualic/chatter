namespace Chatter.Common.Extensions;

public static class DictionaryExtensions
{
	public static T? GetValue<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key) where T : struct => dictionary.TryGetValue(key, out var _value) ? _value : null;
}