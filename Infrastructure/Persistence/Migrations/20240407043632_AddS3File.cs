using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddS3File : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Posts_PostId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "S3Files");

            migrationBuilder.RenameIndex(
                name: "IX_Files_PostId",
                table: "S3Files",
                newName: "IX_S3Files_PostId");

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "S3Files",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "S3Files",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_S3Files",
                table: "S3Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_S3Files_Posts_PostId",
                table: "S3Files",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_S3Files_Posts_PostId",
                table: "S3Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_S3Files",
                table: "S3Files");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "S3Files");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "S3Files");

            migrationBuilder.RenameTable(
                name: "S3Files",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_S3Files_PostId",
                table: "Files",
                newName: "IX_Files_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Posts_PostId",
                table: "Files",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
