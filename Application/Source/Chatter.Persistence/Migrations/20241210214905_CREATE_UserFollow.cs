using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class CREATE_UserFollow : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "UserFollow",
				schema: "dbo",
				columns: table => new
				{
					FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FollowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FollowDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserFollow", x => new { x.FollowerId, x.FollowingId });
					table.ForeignKey(
						name: "FK_UserFollow_User_FollowerId",
						column: x => x.FollowerId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_UserFollow_User_FollowingId",
						column: x => x.FollowingId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserFollow_FollowingId",
				schema: "dbo",
				table: "UserFollow",
				column: "FollowingId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserFollow",
				schema: "dbo");
		}
	}
}