using Chatter.Domain;
using Chatter.Web.Api.ReinforcedTypings.Generator;

namespace Chatter.Web.Api.ReinforcedTypings;

public enum Providers
{
	[EnumProvider<eGender>]
	Genders,

	[EnumProvider<eSystemRole>]
	SystemRoles,
}