using System;

#nullable disable

namespace Chatter.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class CREATE_UserBlob_REMOVE_UserId_From_BLob : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Blob_User_UserId",
				schema: "dbo",
				table: "Blob");

			migrationBuilder.DropIndex(
				name: "IX_Blob_UserId",
				schema: "dbo",
				table: "Blob");

			migrationBuilder.DropColumn(
				name: "IsProfilePhoto",
				schema: "dbo",
				table: "Blob");

			migrationBuilder.DropColumn(
				name: "IsThumbnail",
				schema: "dbo",
				table: "Blob");

			migrationBuilder.DropColumn(
				name: "UserId",
				schema: "dbo",
				table: "Blob");

			migrationBuilder.CreateTable(
				name: "UserBlob",
				schema: "dbo",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsProfilePhoto = table.Column<bool>(type: "bit", nullable: true),
					IsThumbnail = table.Column<bool>(type: "bit", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserBlob", x => new { x.UserId, x.BlobId });
					table.ForeignKey(
						name: "FK_UserBlob_Blob_BlobId",
						column: x => x.BlobId,
						principalSchema: "dbo",
						principalTable: "Blob",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_UserBlob_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserBlob_BlobId",
				schema: "dbo",
				table: "UserBlob",
				column: "BlobId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserBlob",
				schema: "dbo");

			migrationBuilder.AddColumn<bool>(
				name: "IsProfilePhoto",
				schema: "dbo",
				table: "Blob",
				type: "bit",
				nullable: true);

			migrationBuilder.AddColumn<bool>(
				name: "IsThumbnail",
				schema: "dbo",
				table: "Blob",
				type: "bit",
				nullable: true);

			migrationBuilder.AddColumn<Guid>(
				name: "UserId",
				schema: "dbo",
				table: "Blob",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

			migrationBuilder.CreateIndex(
				name: "IX_Blob_UserId",
				schema: "dbo",
				table: "Blob",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_Blob_User_UserId",
				schema: "dbo",
				table: "Blob",
				column: "UserId",
				principalSchema: "dbo",
				principalTable: "User",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}