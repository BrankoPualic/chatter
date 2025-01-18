using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ADD_GroupPhoto_Column_on_Chat_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupImageId",
                schema: "dbo",
                table: "Chat",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_GroupImageId",
                schema: "dbo",
                table: "Chat",
                column: "GroupImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_Blob_GroupImageId",
                schema: "dbo",
                table: "Chat",
                column: "GroupImageId",
                principalSchema: "dbo",
                principalTable: "Blob",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_Blob_GroupImageId",
                schema: "dbo",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_GroupImageId",
                schema: "dbo",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "GroupImageId",
                schema: "dbo",
                table: "Chat");
        }
    }
}
