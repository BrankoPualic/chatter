﻿using Chatter.Domain.Models.Application;
using Chatter.Domain.Models.Application.Messaging;
using Chatter.Domain.Models.Application.Users;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Domain;

public partial interface IDatabaseContext
{
	IDatabaseContext GetDatabaseContext();
}

public partial interface IDatabaseContext : IDatabaseContextBase
{
	DbSet<User> Users { get; }

	DbSet<UserLoginLog> Logins { get; }

	DbSet<UserFollow> Follows { get; }

	DbSet<Blob> Blobs { get; }

	// Messaging

	DbSet<Chat> Chats { get; }

	DbSet<Message> Messages { get; }

	DbSet<ChatMember> ChatMembers { get; }

	DbSet<Attachment> Attachments { get; }
}