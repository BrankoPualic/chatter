using System.Runtime.CompilerServices;

namespace Chatter.Common;

public static partial class Argument
{
	public static void AssertNotNull<T>(T value, [CallerArgumentExpression(nameof(value))] string name = default!)
	{
		if (value is null)
			throw new ArgumentNullException(name);
	}

	public static void AssertNotNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string name = default!) where T : struct
	{
		if (!value.HasValue)
			throw new ArgumentNullException(name);
	}
}