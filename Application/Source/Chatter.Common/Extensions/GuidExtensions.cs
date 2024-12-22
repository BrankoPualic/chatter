namespace Chatter.Common.Extensions;

public static class GuidExtensions
{
	public static bool IsEmpty(this Guid value) => value == Guid.Empty;

	public static bool IsNotEmpty(this Guid value) => value != Guid.Empty;

	public static bool IsEmpty(this Guid? value) => value == null || value == Guid.Empty;

	public static bool IsNotEmpty(this Guid? value) => value != null && value != Guid.Empty;
}