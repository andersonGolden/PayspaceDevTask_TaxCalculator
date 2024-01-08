using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxCalculator.Domain.Migrations
{
    /// <inheritdoc />
    public partial class removedTaxPayerInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxRates_TaxPayerInformation_TaxPayerId",
                table: "TaxRates");

            migrationBuilder.DropTable(
                name: "TaxPayerInformation");

            migrationBuilder.DropIndex(
                name: "IX_TaxRates_TaxPayerId",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "TaxPayerId",
                table: "TaxRates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TaxPayerId",
                table: "TaxRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "TaxPayerInformation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnualIncome = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxPayerInformation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_TaxPayerId",
                table: "TaxRates",
                column: "TaxPayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxRates_TaxPayerInformation_TaxPayerId",
                table: "TaxRates",
                column: "TaxPayerId",
                principalTable: "TaxPayerInformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
