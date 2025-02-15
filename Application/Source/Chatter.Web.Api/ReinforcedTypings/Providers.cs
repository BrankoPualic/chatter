namespace Chatter.Web.Api.ReinforcedTypings;

public enum Providers
{
	[EnumProvider<eSystemRole>]
	SystemRoles,

	[EnumProvider<eGender>]
	Genders,

	[EnumProvider<eMessageStatus>]
	MessageStatuses,

	[EnumProvider<eChatRole>]
	ChatRoles,
}