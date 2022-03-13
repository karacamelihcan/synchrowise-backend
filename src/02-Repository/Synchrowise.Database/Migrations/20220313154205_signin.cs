using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    public partial class signin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Firebase_Id",
                table: "Users",
                newName: "Firebase_uid");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Users",
                newName: "Firebase_Last_Signin_Time");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Firebase_Id",
                table: "Users",
                newName: "IX_Users_Firebase_uid");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<bool>(
                name: "Email_verified",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Firebase_Creation_Time",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Firebase_id_token",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_New_user",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PremiumType",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Signin_Method",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email_verified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Firebase_Creation_Time",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Firebase_id_token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Is_New_user",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PremiumType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Signin_Method",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Firebase_uid",
                table: "Users",
                newName: "Firebase_Id");

            migrationBuilder.RenameColumn(
                name: "Firebase_Last_Signin_Time",
                table: "Users",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Firebase_uid",
                table: "Users",
                newName: "IX_Users_Firebase_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
