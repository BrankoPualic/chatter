using Chatter.Domain.Models.Application.Users;

namespace Chatter.Persistence;

public partial class DatabaseContext : IDatabaseContext
{
	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserLoginLog> Logins { get; set; }

	public virtual DbSet<UserFollow> Follows { get; set; }
}