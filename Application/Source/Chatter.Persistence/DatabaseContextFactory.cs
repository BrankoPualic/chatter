using Microsoft.EntityFrameworkCore.Design;

namespace Chatter.Persistence;

internal class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
	public DatabaseContext CreateDbContext(string[] args)
	{
		IIdentityUser identityUser = null;
		return new DatabaseContext(identityUser);
	}
}