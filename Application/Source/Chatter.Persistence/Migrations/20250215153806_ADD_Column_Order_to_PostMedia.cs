using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ADD_Column_Order_to_PostMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "dbo",
                table: "PostMedia",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                schema: "dbo",
                table: "PostMedia");
        }
    }
}
