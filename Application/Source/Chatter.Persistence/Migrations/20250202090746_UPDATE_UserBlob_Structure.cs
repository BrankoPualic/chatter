using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_UserBlob_Structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProfilePhoto",
                schema: "dbo",
                table: "UserBlob");

            migrationBuilder.DropColumn(
                name: "IsThumbnail",
                schema: "dbo",
                table: "UserBlob");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                schema: "dbo",
                table: "UserBlob",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                schema: "dbo",
                table: "UserBlob");

            migrationBuilder.AddColumn<bool>(
                name: "IsProfilePhoto",
                schema: "dbo",
                table: "UserBlob",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsThumbnail",
                schema: "dbo",
                table: "UserBlob",
                type: "bit",
                nullable: true);
        }
    }
}
