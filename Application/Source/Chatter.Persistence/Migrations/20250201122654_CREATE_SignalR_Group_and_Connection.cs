using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_SignalR_Group_and_Connection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                schema: "dbo",
                columns: table => new
                {
                    ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "Connection",
                schema: "dbo",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connection", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_Connection_Group_GroupChatId",
                        column: x => x.GroupChatId,
                        principalSchema: "dbo",
                        principalTable: "Group",
                        principalColumn: "ChatId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connection_GroupChatId",
                schema: "dbo",
                table: "Connection",
                column: "GroupChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connection",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Group",
                schema: "dbo");
        }
    }
}
