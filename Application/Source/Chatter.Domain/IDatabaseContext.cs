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
}