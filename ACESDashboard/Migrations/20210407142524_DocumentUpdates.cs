using Microsoft.EntityFrameworkCore.Migrations;

namespace ACESDashboard.Migrations
{
    public partial class DocumentUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentFileName",
                table: "Documents",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "FileContentType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileContentType",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Documents",
                newName: "DocumentFileName");
        }
    }
}
