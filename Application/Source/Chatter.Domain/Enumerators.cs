using System.ComponentModel;

namespace Chatter.Domain;

public enum eBlobType
{
	Image = 100,
	Video = 200
}

public enum eChatRole
{
	[Description("")]
	NotSet = 0,

	[Description("Member")]
	Member = 10,

	[Description("Admin")]
	Admin = 20,

	[Description("Moderator")]
	Moderator = 30
}

public enum eGender
{
	[Description("")]
	NotSet = 0,

	Male,
	Female,
	Other
}

public enum eMessageStatus
{
	Sent = 0,
	Delivered = 1,
	Seen = 2
}

public enum eSystemRole
{
	[Description("System Administrator")]
	SystemAdministrator = 1,

	[Description("Member")]
	Member = 2,

	[Description("User Admin")]
	UserAdmin = 3,

	[Description("Moderator")]
	Moderator = 4,

	[Description("Legal Department")]
	LegalDepartment = 5
}

public enum eMessageType
{
	Text = 0,
	Image = 100,
	Video = 200,
	Document = 300,
	Voice = 400
}