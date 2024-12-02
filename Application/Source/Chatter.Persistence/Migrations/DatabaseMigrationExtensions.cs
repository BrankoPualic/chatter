using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chatter.Persistence.Migrations;

internal static class DatabaseMigrationExtensions
{
	internal static string GetLookupViewName(Type enumType) => $"vw_{enumType.Name.TrimStart('e')}_lu";

	internal static string GetEnumLookupCreateScript(this IEnumerable<Type> enumTypes)
	{
		var result = new List<string>();

		foreach (var enumType in enumTypes.Distinct())
		{
			static string GetDisplayName<T>(T enumerator, string value) where T : Type
			{
				if (value == null)
					return value;

				var fieldInfo = enumerator.GetField(value);

				var displayAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().ToArray();

				if (displayAttributes.Length > 0)
					return displayAttributes.First().GetName();

				var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>().ToArray();

				return descriptionAttributes.Length > 0 ? descriptionAttributes.First().Description : value;
			}

			var enumNames = Enum.GetValues(enumType)
				.Cast<int>()
				.ToDictionary(_ => _, _ => Enum.GetName(enumType, _));

			result.Add($"CREATE OR ALTER VIEW[dbo].[{GetLookupViewName(enumType)}] AS\r\n" +
				"SELECT * FROM (\r\n" +
				"\tVALUES\r\n" +
				string.Join(",\r\n", enumNames.Select(_ => $"\t\t({_.Key}, '{_.Value.Replace("'", "''")}', '{GetDisplayName(enumType, _.Value).Replace("'", "''")}')")) +
				$") AS CD([ID], [Value], [Description]); \r\nGO\r\n");
		}

		return string.Join("\r\n", result);
	}

	internal static string GetEnumLookupDropScript(this IEnumerable<Type> enumTypes)
	{
		var result = new List<string>();

		foreach (var enumType in enumTypes)
		{
			result.Add($"DROP VIEW IF EXISTS [dbo].[{GetLookupViewName(enumType)}];\r\nGO");
		}
		return string.Join("\r\n", result);
	}

	internal static void Up(this IEnumerable<IDatabaseView> views, Func<string, bool, OperationBuilder<SqlOperation>> sql) => views.ToList().ForEach(_ => sql(_.Script, false));

	internal static void Down(this IEnumerable<IDatabaseView> views, Func<string, bool, OperationBuilder<SqlOperation>> sql) => views.ToList().ForEach(_ => sql(_.DropScript, false));

	internal static void Seed(this MigrationBuilder migrationBuilder, string file)
	{
		var path = Path.Combine("Scripts", "Seeds", file);
		var sql = File.ReadAllText(path);
		migrationBuilder.Sql(sql);
	}

	// Indexes
	internal static void Up(this MigrationBuilder migrationBuilder, IDatabaseIndex index) => migrationBuilder.Sql(index.DropSql + "\n" + index.Sql);

	internal static void Down(this MigrationBuilder migrationBuilder, IDatabaseIndex index) => migrationBuilder.Sql(index.DropSql);

	internal static void Up(this MigrationBuilder migrationBuilder, IEnumerable<IDatabaseIndex> indexes) => indexes.ToList().ForEach(_ => migrationBuilder.Sql(_.DropSql + "\n" + _.Sql));

	internal static void Down(this MigrationBuilder migrationBuilder, IEnumerable<IDatabaseIndex> indexes) => indexes.ToList().ForEach(_ => migrationBuilder.Sql(_.DropSql));
}