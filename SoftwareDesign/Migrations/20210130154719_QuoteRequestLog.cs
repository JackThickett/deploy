using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareDesign.Migrations
{
    public partial class QuoteRequestLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuoteRequestLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Postcode = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    EstimateUsage = table.Column<int>(nullable: false),
                    PaymentMethod = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    userID = table.Column<string>(nullable: true),
                    SnapShowTaken = table.Column<DateTime>(nullable: false),
                    VersionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteRequestLog", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuoteRequestLog");
        }
    }
}
