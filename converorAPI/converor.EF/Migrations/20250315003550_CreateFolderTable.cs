using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace converor.EF.Migrations
{
    /// <inheritdoc />
    public partial class CreateFolderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDescriptions_AspNetUsers_UserId",
                table: "FileDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_FileDescriptions_UserId",
                table: "FileDescriptions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FileDescriptions");

            migrationBuilder.AddColumn<int>(
                name: "ParentFolderId",
                table: "FileDescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentFolderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileDescriptions_ParentFolderId",
                table: "FileDescriptions",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ApplicationUserId",
                table: "Folders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDescriptions_Folders_ParentFolderId",
                table: "FileDescriptions",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDescriptions_Folders_ParentFolderId",
                table: "FileDescriptions");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_FileDescriptions_ParentFolderId",
                table: "FileDescriptions");

            migrationBuilder.DropColumn(
                name: "ParentFolderId",
                table: "FileDescriptions");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FileDescriptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FileDescriptions_UserId",
                table: "FileDescriptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDescriptions_AspNetUsers_UserId",
                table: "FileDescriptions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
