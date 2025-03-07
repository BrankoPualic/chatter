﻿namespace Chatter.Common;

public class Error
{
	public Error() => Errors = new Dictionary<string, List<string>>();

	public Error(string message) : this() => Errors.Add("Error", [message]);

	public Error(string key, string message) : this() => Errors.Add(key, [message]);

	public Error(string key, List<string> messages) : this() => Errors.Add(key, messages);

	public IDictionary<string, List<string>> Errors { get; }

	public bool HasErrors => Errors.Any();

	public void AddError(string key, string message)
	{
		if (!Errors.ContainsKey(key))
			Errors[key] = [];

		Errors[key].Add(message);
	}

	public void AddErrors(string key, List<string> messages) => messages.ForEach(_ => AddError(key, _));

	public List<string> GetValue(string key) => Errors.TryGetValue(key, out List<string>? value) ? value : [];
}