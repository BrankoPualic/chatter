#nullable disable

namespace Chatter.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class CREATE_Post_and_Comment_Feature_tables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Post",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TypeId = table.Column<int>(type: "int", nullable: false),
					IsCommentsDisabled = table.Column<bool>(type: "bit", nullable: false),
					IsArchived = table.Column<bool>(type: "bit", nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Post", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Comment",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Comment", x => x.Id);
					table.ForeignKey(
						name: "FK_Comment_Comment_ParentId",
						column: x => x.ParentId,
						principalSchema: "dbo",
						principalTable: "Comment",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Comment_Post_PostId",
						column: x => x.PostId,
						principalSchema: "dbo",
						principalTable: "Post",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Comment_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "PostLike",
				schema: "dbo",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PostLike", x => new { x.UserId, x.PostId });
					table.ForeignKey(
						name: "FK_PostLike_Post_PostId",
						column: x => x.PostId,
						principalSchema: "dbo",
						principalTable: "Post",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_PostLike_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "PostMedia",
				schema: "dbo",
				columns: table => new
				{
					PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PostMedia", x => new { x.PostId, x.BlobId });
					table.ForeignKey(
						name: "FK_PostMedia_Blob_BlobId",
						column: x => x.BlobId,
						principalSchema: "dbo",
						principalTable: "Blob",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_PostMedia_Post_PostId",
						column: x => x.PostId,
						principalSchema: "dbo",
						principalTable: "Post",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "CommentLike",
				schema: "dbo",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CommentLike", x => new { x.UserId, x.CommentId });
					table.ForeignKey(
						name: "FK_CommentLike_Comment_CommentId",
						column: x => x.CommentId,
						principalSchema: "dbo",
						principalTable: "Comment",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_CommentLike_User_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "User",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Comment_ParentId",
				schema: "dbo",
				table: "Comment",
				column: "ParentId");

			migrationBuilder.CreateIndex(
				name: "IX_Comment_PostId",
				schema: "dbo",
				table: "Comment",
				column: "PostId");

			migrationBuilder.CreateIndex(
				name: "IX_Comment_UserId",
				schema: "dbo",
				table: "Comment",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_CommentLike_CommentId",
				schema: "dbo",
				table: "CommentLike",
				column: "CommentId");

			migrationBuilder.CreateIndex(
				name: "IX_PostLike_PostId",
				schema: "dbo",
				table: "PostLike",
				column: "PostId");

			migrationBuilder.CreateIndex(
				name: "IX_PostMedia_BlobId",
				schema: "dbo",
				table: "PostMedia",
				column: "BlobId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CommentLike",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "PostLike",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "PostMedia",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Comment",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Post",
				schema: "dbo");
		}
	}
}