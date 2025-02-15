using Chatter.Common.Extensions;

namespace Chatter.Common;

public static class Functions
{
	public static Guid AssignGuid(Guid value) => value.IsEmpty() ? Guid.NewGuid() : value;
}