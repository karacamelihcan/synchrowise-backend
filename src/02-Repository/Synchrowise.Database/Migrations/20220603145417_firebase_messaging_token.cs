using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    public partial class firebase_messaging_token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "firebase_messaging_token",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firebase_messaging_token",
                table: "Users");
        }
    }
}
