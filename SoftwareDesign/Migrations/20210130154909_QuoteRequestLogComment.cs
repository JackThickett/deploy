using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareDesign.Migrations
{
    public partial class QuoteRequestLogComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "QuoteRequestLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "QuoteRequestLog");
        }
    }
}
