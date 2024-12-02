using Chatter.Persistence.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatter.Persistence;

public abstract class ViewsMigration : Migration
{
	private readonly Type[] _enums =
	[
		typeof(eSystemRole),
	];

	private readonly IDatabaseView[] views = [];

	protected override void Up(MigrationBuilder migrationBuilder)

	{
		migrationBuilder.Sql(_enums.GetEnumLookupCreateScript());

		views.Up(migrationBuilder.Sql);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(_enums.GetEnumLookupDropScript());

		views.Down(migrationBuilder.Sql);
	}
}