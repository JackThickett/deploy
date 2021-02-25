using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareDesign.Migrations
{
    public partial class QouteRequestAdditionalFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimateUsage",
                table: "QuoteRequestLog");

            migrationBuilder.DropColumn(
                name: "EstimateUsage",
                table: "QuoteRequest");

            migrationBuilder.AddColumn<string>(
                name: "EnergyType",
                table: "QuoteRequestLog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimateElectricityUsage",
                table: "QuoteRequestLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstimateGasUsage",
                table: "QuoteRequestLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EnergyType",
                table: "QuoteRequest",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EstimateElectricityUsage",
                table: "QuoteRequest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstimateGasUsage",
                table: "QuoteRequest",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyType",
                table: "QuoteRequestLog");

            migrationBuilder.DropColumn(
                name: "EstimateElectricityUsage",
                table: "QuoteRequestLog");

            migrationBuilder.DropColumn(
                name: "EstimateGasUsage",
                table: "QuoteRequestLog");

            migrationBuilder.DropColumn(
                name: "EnergyType",
                table: "QuoteRequest");

            migrationBuilder.DropColumn(
                name: "EstimateElectricityUsage",
                table: "QuoteRequest");

            migrationBuilder.DropColumn(
                name: "EstimateGasUsage",
                table: "QuoteRequest");

            migrationBuilder.AddColumn<int>(
                name: "EstimateUsage",
                table: "QuoteRequestLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstimateUsage",
                table: "QuoteRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
