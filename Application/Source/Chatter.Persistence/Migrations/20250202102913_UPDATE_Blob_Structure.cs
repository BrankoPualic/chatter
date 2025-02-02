using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_Blob_Structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "dbo",
                table: "Blob");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                schema: "dbo",
                table: "Blob",
                newName: "Type");

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                schema: "dbo",
                table: "Blob",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "dbo",
                table: "Blob",
                newName: "MimeType");

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                schema: "dbo",
                table: "Blob",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                schema: "dbo",
                table: "Blob",
                type: "int",
                nullable: true);
        }
    }
}
