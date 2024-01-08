using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxCalculator.Domain.Migrations
{
    /// <inheritdoc />
    public partial class trimmedTaxInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "TaxInformation");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "TaxInformation");

            migrationBuilder.DropColumn(
                name: "To",
                table: "TaxInformation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "From",
                table: "TaxInformation",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "TaxInformation",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "To",
                table: "TaxInformation",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
