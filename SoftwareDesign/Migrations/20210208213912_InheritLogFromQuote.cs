using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareDesign.Migrations
{
    public partial class InheritLogFromQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuoteLog_Quote_quoteSnapshotID",
                table: "QuoteLog");

            migrationBuilder.DropTable(
                name: "QuoteRequestsLog");

            migrationBuilder.DropIndex(
                name: "IX_QuoteLog_quoteSnapshotID",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "quoteSnapshotID",
                table: "QuoteLog");

            migrationBuilder.AddColumn<int>(
                name: "ContractID",
                table: "QuoteLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "QuoteLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "QuoteLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EstimatedElectricityUsage",
                table: "QuoteLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedGasUsage",
                table: "QuoteLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "QuoteLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedContractType",
                table: "QuoteLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedMeterType",
                table: "QuoteLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedPaymentFrequency",
                table: "QuoteLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "QuoteLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "QuoteLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userID",
                table: "QuoteLog",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedPaymentFrequency",
                table: "Quote",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedMeterType",
                table: "Quote",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedContractType",
                table: "Quote",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "Quote",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractID",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "EstimatedElectricityUsage",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "EstimatedGasUsage",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "SelectedContractType",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "SelectedMeterType",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "SelectedPaymentFrequency",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "QuoteLog");

            migrationBuilder.DropColumn(
                name: "userID",
                table: "QuoteLog");

            migrationBuilder.AddColumn<int>(
                name: "quoteSnapshotID",
                table: "QuoteLog",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedPaymentFrequency",
                table: "Quote",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "SelectedMeterType",
                table: "Quote",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "SelectedContractType",
                table: "Quote",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "Quote",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "QuoteRequestsLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnergyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimateElectricityUsage = table.Column<int>(type: "int", nullable: false),
                    EstimateGasUsage = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteRequestsLog", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteLog_quoteSnapshotID",
                table: "QuoteLog",
                column: "quoteSnapshotID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteLog_Quote_quoteSnapshotID",
                table: "QuoteLog",
                column: "quoteSnapshotID",
                principalTable: "Quote",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
