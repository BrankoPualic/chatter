﻿using System.ComponentModel;

namespace Chatter.Domain;

public enum eBlobType
{
	[Description("")]
	NotSet = 0,

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

	Male = 1,
	Female = 2,
	Other = 3
}

public enum eMessageStatus
{
	[Description("")]
	NotSet = 0,

	Draft = 1,

	[CssClass("fa-solid fa-check gray")]
	Sent = 2,

	[CssClass("fa-solid fa-check primary-red")]
	Delivered = 3,

	[CssClass("double-check")]
	Seen = 4,

	[CssClass("fa-solid fa-share small")]
	Forwarded = 5
}

public enum eSystemRole
{
	[Description("")]
	NotSet = 0,

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
	[Description("")]
	NotSet = 0,

	Text = 1,
	Image = 100,
	Video = 200,
	Document = 300,
	Voice = 400
}

public enum eUserMediaType
{
	[Description("")]
	NotSet = 0,

	[Description("Profile Photo")]
	ProfilePhoto = 1,

	[Description("Thumbnail")]
	Thumbnail = 2,
}

public enum ePostType
{
	[Description("")]
	NotSet = 0,

	[Description("Text")]
	Text = 1,

	[Description("Image")]
	Image = 2,

	[Description("Video")]
	Video = 3,
}