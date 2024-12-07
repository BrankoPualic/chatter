using Chatter.Domain.Models.Application.Users;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class CREATE_User_UserLoginLog : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "dbo");

			migrationBuilder.CreateTable(
				name: "User",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
					FullName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, computedColumnSql: "[FirstName] + ' ' + [LastName]"),
					Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
					Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					IsPrivate = table.Column<bool>(type: "bit", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					IsLocked = table.Column<bool>(type: "bit", nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_User", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserLoginLog",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserLoginLog", x => x.Id);
					table.ForeignKey(
						name: "FK_UserLoginLog_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "UserRole",
				schema: "dbo",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					RoleId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_UserRole_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserLoginLog_UserId",
				schema: "dbo",
				table: "UserLoginLog",
				column: "UserId");

			migrationBuilder.Up([User.IX_User_Email, User.IX_User_Username, User.IX_User_IsActive_IsLocked]);

			migrationBuilder.Seed("Users.sql");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserLoginLog",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "UserRole",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "User",
				schema: "dbo");
		}
	}
}