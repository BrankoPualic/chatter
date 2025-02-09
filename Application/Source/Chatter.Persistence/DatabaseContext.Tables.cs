using Chatter.Domain.Models.Application;
using Chatter.Domain.Models.Application.Messaging;
using Chatter.Domain.Models.Application.Posts;
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

	// Posts

	public virtual DbSet<Post> Posts { get; set; }

	public virtual DbSet<PostMedia> PostMedia { get; set; }

	public virtual DbSet<PostLike> PostLikes { get; set; }

	public virtual DbSet<Comment> Comments { get; set; }

	public virtual DbSet<CommentLike> CommentLikes { get; set; }
}