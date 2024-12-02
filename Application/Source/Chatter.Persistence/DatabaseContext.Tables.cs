using Chatter.Domain.Models.Application.Users;

namespace Chatter.Persistence;

public partial class DatabaseContext : IDatabaseContext
{
	public virtual DbSet<User> Users { get; set; }
}