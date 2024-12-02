namespace Chatter.Domain.Models;

public abstract class BaseIndexAuditedDomain<T> : BaseAuditedDomain
	where T : BaseIndexAuditedDomain<T>
{
	private static readonly Lazy<string> _tableName = new(ModelExtensions.GetFullTableName<T>);

	public static string TableName => _tableName.Value;

	protected class DatabaseIndex(string indexName) : Domain.DatabaseIndex(indexName, TableName)
	{ }
}