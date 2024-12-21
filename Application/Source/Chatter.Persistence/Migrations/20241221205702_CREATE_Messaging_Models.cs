using Chatter.Domain.Models.Application.Messaging;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class CREATE_Messaging_Models : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Chat",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
					IsGroup = table.Column<bool>(type: "bit", nullable: false),
					LastMessageOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Chat", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Message",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
					TypeId = table.Column<int>(type: "int", nullable: false),
					Status = table.Column<int>(type: "int", nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Message", x => x.Id);
					table.ForeignKey(
						name: "FK_Message_Chat_ChatId",
						column: x => x.ChatId,
						principalSchema: "dbo",
						principalTable: "Chat",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Message_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "Attachment",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					Order = table.Column<int>(type: "int", nullable: true),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Attachment", x => x.Id);
					table.ForeignKey(
						name: "FK_Attachment_Blob_BlobId",
						column: x => x.BlobId,
						principalSchema: "dbo",
						principalTable: "Blob",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Attachment_Message_MessageId",
						column: x => x.MessageId,
						principalSchema: "dbo",
						principalTable: "Message",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "ChatMember",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					RoleId = table.Column<int>(type: "int", nullable: true),
					IsMuted = table.Column<bool>(type: "bit", nullable: false),
					LastReadMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ChatMember", x => x.Id);
					table.ForeignKey(
						name: "FK_ChatMember_Chat_ChatId",
						column: x => x.ChatId,
						principalSchema: "dbo",
						principalTable: "Chat",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_ChatMember_Message_LastReadMessageId",
						column: x => x.LastReadMessageId,
						principalSchema: "dbo",
						principalTable: "Message",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_ChatMember_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Attachment_BlobId",
				schema: "dbo",
				table: "Attachment",
				column: "BlobId");

			migrationBuilder.CreateIndex(
				name: "IX_Attachment_MessageId",
				schema: "dbo",
				table: "Attachment",
				column: "MessageId");

			migrationBuilder.CreateIndex(
				name: "IX_ChatMember_ChatId",
				schema: "dbo",
				table: "ChatMember",
				column: "ChatId");

			migrationBuilder.CreateIndex(
				name: "IX_ChatMember_LastReadMessageId",
				schema: "dbo",
				table: "ChatMember",
				column: "LastReadMessageId");

			migrationBuilder.CreateIndex(
				name: "IX_ChatMember_UserId",
				schema: "dbo",
				table: "ChatMember",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Message_ChatId",
				schema: "dbo",
				table: "Message",
				column: "ChatId");

			migrationBuilder.CreateIndex(
				name: "IX_Message_UserId",
				schema: "dbo",
				table: "Message",
				column: "UserId");

			migrationBuilder.Up(Chat.IX_Chat_IsGroup_LastMessageOn);
			migrationBuilder.Up([Message.IX_Message_TypeId, Message.IX_Message_ChatId_CreatedAt]);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Down(Chat.IX_Chat_IsGroup_LastMessageOn);
			migrationBuilder.Down([Message.IX_Message_TypeId, Message.IX_Message_ChatId_CreatedAt]);

			migrationBuilder.DropTable(
				name: "Attachment",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "ChatMember",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Message",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Chat",
				schema: "dbo");
		}
	}
}