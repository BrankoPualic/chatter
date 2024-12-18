﻿using System.ComponentModel;

namespace Chatter.Domain;

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

public enum eGender
{
	[Description("")]
	NotSet = 0,

	Male,
	Female,
	Other
}

public enum eBlobType
{
	Image = 100,
	Video = 200
}