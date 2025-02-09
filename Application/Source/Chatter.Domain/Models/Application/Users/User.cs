using Chatter.Domain.Interfaces;
using Chatter.Domain.Models.Application.Messaging;
using Chatter.Domain.Models.Application.Posts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Users;

public class User : BaseIndexAuditedDomain<User>, IConfigurableEntity
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	public string FullName { get; private set; }

	public string Username { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public eGender? GenderId { get; set; }

	public bool IsPrivate { get; set; }

	public bool IsActive { get; set; }

	public bool IsLocked { get; set; }

	[InverseProperty(nameof(UserRole.User))]
	public virtual ICollection<UserRole> Roles { get; set; } = [];

	[InverseProperty(nameof(UserLoginLog.User))]
	public virtual ICollection<UserLoginLog> Logins { get; set; } = [];

	[InverseProperty(nameof(UserFollow.Follower))]
	public virtual ICollection<UserFollow> Following { get; set; } = [];

	[InverseProperty(nameof(UserFollow.Following))]
	public virtual ICollection<UserFollow> Followers { get; set; } = [];

	[InverseProperty(nameof(UserBlob.User))]
	public virtual ICollection<UserBlob> Blobs { get; set; } = [];

	[InverseProperty(nameof(ChatMember.User))]
	public virtual ICollection<ChatMember> ChatParticipations { get; set; } = [];

	[InverseProperty(nameof(Post.User))]
	public virtual ICollection<Post> Posts { get; set; } = [];

	[InverseProperty(nameof(Comment.User))]
	public virtual ICollection<Comment> Comments { get; set; } = [];

	//
	// Indexes
	//

	public static IDatabaseIndex IX_User_Email => new DatabaseIndex(nameof(IX_User_Email))
	{
		Columns = { nameof(Email) },
		IsUnique = true,
	};

	public static IDatabaseIndex IX_User_Username => new DatabaseIndex(nameof(IX_User_Username))
	{
		Columns = { nameof(Username) },
		IsUnique = true,
	};

	public static IDatabaseIndex IX_User_IsActive_IsLocked => new DatabaseIndex(nameof(IX_User_IsActive_IsLocked))
	{
		Columns = { nameof(IsActive), nameof(IsLocked) },
		Include = { nameof(Username), nameof(FirstName), nameof(LastName) },
	};

	// Configuration

	public void Configure(ModelBuilder builder)
	{
		builder.Entity<User>(_ =>
		{
			_.Property(_ => _.FirstName).HasMaxLength(20).IsRequired();
			_.Property(_ => _.LastName).HasMaxLength(30).IsRequired();
			_.Property(_ => _.Username).HasMaxLength(20).IsRequired();
			_.Property(_ => _.Email).HasMaxLength(80).IsRequired();
			_.Property(_ => _.FullName).HasMaxLength(70).HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
		});
	}
}