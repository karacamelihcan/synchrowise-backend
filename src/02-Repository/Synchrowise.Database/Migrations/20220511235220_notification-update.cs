using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    public partial class notificationupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupNotification",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MessageNotification",
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

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageNotification = table.Column<bool>(type: "boolean", nullable: false),
                    GroupNotification = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerUserId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Notifications_UserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Notifications");

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

            migrationBuilder.AddColumn<bool>(
                name: "GroupNotification",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MessageNotification",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
