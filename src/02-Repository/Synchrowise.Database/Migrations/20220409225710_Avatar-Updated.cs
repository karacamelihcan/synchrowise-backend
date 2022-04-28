using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    public partial class AvatarUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserImages");

            migrationBuilder.CreateTable(
                name: "UserAvatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerID = table.Column<int>(type: "integer", nullable: false),
                    OwnerGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvatars", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarID",
                table: "Users",
                column: "AvatarID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_Path",
                table: "UserAvatars",
                column: "Path",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserAvatars_AvatarID",
                table: "Users",
                column: "AvatarID",
                principalTable: "UserAvatars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserAvatars_AvatarID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.DropIndex(
                name: "IX_Users_AvatarID",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerID = table.Column<int>(type: "integer", nullable: false),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImages_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_OwnerID",
                table: "UserImages",
                column: "OwnerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_Path",
                table: "UserImages",
                column: "Path",
                unique: true);
        }
    }
}
