using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    public partial class usermodal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerUserId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Notifications_UserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "Groups",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_OwnerUserId",
                table: "Groups",
                newName: "IX_Groups_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Notifications_Id",
                table: "Users",
                column: "Id",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Notifications_Id",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Groups",
                newName: "OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                newName: "IX_Groups_OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerUserId",
                table: "Groups",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Notifications_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
