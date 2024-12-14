using Chatter.Domain.Models.Application;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class CREATE_Blob : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ProfileImageUrl",
				schema: "dbo",
				table: "User");

			migrationBuilder.DropColumn(
				name: "PublicId",
				schema: "dbo",
				table: "User");

			migrationBuilder.CreateTable(
				name: "Blob",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TypeId = table.Column<int>(type: "int", nullable: false),
					MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					IsProfilePhoto = table.Column<bool>(type: "bit", nullable: true),
					IsThumbnail = table.Column<bool>(type: "bit", nullable: true),
					Size = table.Column<long>(type: "bigint", nullable: false),
					Duration = table.Column<int>(type: "int", nullable: true),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Blob", x => x.Id);
					table.ForeignKey(
						name: "FK_Blob_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Blob_UserId",
				schema: "dbo",
				table: "Blob",
				column: "UserId");

			migrationBuilder.Up(Blob.IX_Blob_TypeId);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Down(Blob.IX_Blob_TypeId);

			migrationBuilder.DropTable(
				name: "Blob",
				schema: "dbo");

			migrationBuilder.AddColumn<string>(
				name: "ProfileImageUrl",
				schema: "dbo",
				table: "User",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "PublicId",
				schema: "dbo",
				table: "User",
				type: "nvarchar(max)",
				nullable: true);
		}
	}
}