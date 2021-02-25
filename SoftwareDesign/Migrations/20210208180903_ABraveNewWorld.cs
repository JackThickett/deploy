using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareDesign.Migrations
{
    public partial class ABraveNewWorld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuoteRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteRequestLog",
                table: "QuoteRequestLog");

            migrationBuilder.RenameTable(
                name: "QuoteRequestLog",
                newName: "QuoteRequestsLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteRequestsLog",
                table: "QuoteRequestsLog",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GasRate = table.Column<float>(nullable: false),
                    ElectricityRate = table.Column<float>(nullable: false),
                    ContractName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Quote",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ContractID = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    EstimatedElectricityUsage = table.Column<int>(nullable: false),
                    EstimatedGasUsage = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    SelectedContractType = table.Column<string>(nullable: true),
                    SelectedMeterType = table.Column<string>(nullable: true),
                    SelectedPaymentFrequency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quote", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "QuoteLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quoteSnapshotID = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    staffID = table.Column<string>(nullable: true),
                    Occured = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuoteLog_Quote_quoteSnapshotID",
                        column: x => x.quoteSnapshotID,
                        principalTable: "Quote",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteLog_quoteSnapshotID",
                table: "QuoteLog",
                column: "quoteSnapshotID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "QuoteLog");

            migrationBuilder.DropTable(
                name: "Quote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteRequestsLog",
                table: "QuoteRequestsLog");

            migrationBuilder.RenameTable(
                name: "QuoteRequestsLog",
                newName: "QuoteRequestLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteRequestLog",
                table: "QuoteRequestLog",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "QuoteRequest",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnergyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimateElectricityUsage = table.Column<int>(type: "int", nullable: false),
                    EstimateGasUsage = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteRequest", x => x.ID);
                });
        }
    }
}
