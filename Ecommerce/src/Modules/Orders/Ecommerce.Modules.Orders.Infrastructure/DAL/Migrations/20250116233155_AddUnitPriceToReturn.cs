using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPriceToReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "orders",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "orders",
                table: "ReturnProducts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                schema: "orders",
                table: "ReturnProducts",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ReturnProduct_PriceComparison",
                schema: "orders",
                table: "ReturnProducts",
                sql: "\"UnitPrice\" <= \"Price\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ReturnProduct_UnitPrice",
                schema: "orders",
                table: "ReturnProducts",
                sql: "\"UnitPrice\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "orders",
                table: "Products",
                sql: "\"Quantity\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ReturnProduct_PriceComparison",
                schema: "orders",
                table: "ReturnProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ReturnProduct_UnitPrice",
                schema: "orders",
                table: "ReturnProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "orders",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "orders",
                table: "ReturnProducts");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                schema: "orders",
                table: "ReturnProducts");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "orders",
                table: "Products",
                sql: "\"Quantity\" > 0");
        }
    }
}
