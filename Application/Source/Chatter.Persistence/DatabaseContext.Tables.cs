using Chatter.Domain.Models.Application;
using Chatter.Domain.Models.Application.Messaging;
using Chatter.Domain.Models.Application.SignalR;
using Chatter.Domain.Models.Application.Users;

namespace Chatter.Persistence;

public partial class DatabaseContext : IDatabaseContext
{
	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserLoginLog> Logins { get; set; }

	public virtual DbSet<UserFollow> Follows { get; set; }

	public virtual DbSet<UserBlob> UserBlobs { get; set; }

	public virtual DbSet<Blob> Blobs { get; set; }

	// Messaging

	public virtual DbSet<Chat> Chats { get; set; }

	public virtual DbSet<Message> Messages { get; set; }

	public virtual DbSet<ChatMember> ChatMembers { get; set; }

	public virtual DbSet<Attachment> Attachments { get; set; }

	// SignalR

	public virtual DbSet<Group> Groups { get; set; }

	public virtual DbSet<Connection> Connections { get; set; }
}