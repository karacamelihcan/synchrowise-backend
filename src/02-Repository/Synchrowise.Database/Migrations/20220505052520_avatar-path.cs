using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    public partial class avatarpath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAvatars_Path",
                table: "UserAvatars");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_Path",
                table: "UserAvatars",
                column: "Path",
                unique: true);
        }
    }
}
