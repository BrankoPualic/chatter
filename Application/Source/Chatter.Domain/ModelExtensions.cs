using Chatter.Common.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain;

public static class ModelExtensions
{
	public static string GetTableName(this Type type) => (Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute)?.Name ?? type.Name;

	public static string GetFullTableName<TDatabaseEntity>()
	{
		var type = typeof(TDatabaseEntity);
		var attr = Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute;
		return (attr?.Name) == null
			? type.Name
			: new[] { attr.Schema, attr.Name }!.Join(".");
	}
}