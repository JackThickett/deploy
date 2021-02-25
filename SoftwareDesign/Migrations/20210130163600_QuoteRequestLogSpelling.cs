using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareDesign.Migrations
{
    public partial class QuoteRequestLogSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnapShowTaken",
                table: "QuoteRequestLog");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "QuoteRequestLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "QuoteID",
                table: "QuoteRequestLog",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "QuoteRequestLog");

            migrationBuilder.DropColumn(
                name: "QuoteID",
                table: "QuoteRequestLog");

            migrationBuilder.AddColumn<DateTime>(
                name: "SnapShowTaken",
                table: "QuoteRequestLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
