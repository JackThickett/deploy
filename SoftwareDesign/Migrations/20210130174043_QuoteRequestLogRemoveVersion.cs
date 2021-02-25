using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareDesign.Migrations
{
    public partial class QuoteRequestLogRemoveVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VersionID",
                table: "QuoteRequestLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VersionID",
                table: "QuoteRequestLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
