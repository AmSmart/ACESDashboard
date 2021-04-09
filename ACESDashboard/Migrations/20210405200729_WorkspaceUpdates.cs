using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ACESDashboard.Migrations
{
    public partial class WorkspaceUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Updates");

            migrationBuilder.DropColumn(
                name: "UpdateType",
                table: "Updates");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Updates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Updates");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Updates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateType",
                table: "Updates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
