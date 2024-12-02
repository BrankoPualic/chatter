namespace Chatter.Persistence;

internal static class DatabaseContextExtensions
{
	internal static void SetTableNames(this ModelBuilder builder)
	{
		foreach (var type in builder.Model.GetEntityTypes())
		{
			type.SetTableName(type.ClrType.GetTableName());
		}
	}
}