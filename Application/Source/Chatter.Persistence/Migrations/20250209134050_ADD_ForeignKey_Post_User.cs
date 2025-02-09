#nullable disable

namespace Chatter.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class ADD_ForeignKey_Post_User : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_Post_UserId",
				schema: "dbo",
				table: "Post",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_Post_User_UserId",
				schema: "dbo",
				table: "Post",
				column: "UserId",
				principalSchema: "dbo",
				principalTable: "User",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Post_User_UserId",
				schema: "dbo",
				table: "Post");

			migrationBuilder.DropIndex(
				name: "IX_Post_UserId",
				schema: "dbo",
				table: "Post");
		}
	}
}