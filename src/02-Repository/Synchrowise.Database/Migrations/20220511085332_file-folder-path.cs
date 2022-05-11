using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    public partial class filefolderpath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "UserAvatars",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "FolderPath",
                table: "UserAvatars",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FolderPath",
                table: "GroupFiles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderPath",
                table: "UserAvatars");

            migrationBuilder.DropColumn(
                name: "FolderPath",
                table: "GroupFiles");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "UserAvatars",
                newName: "Path");
        }
    }
}
