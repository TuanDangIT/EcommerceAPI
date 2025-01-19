using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DropPriceComparisonCheckConstraintAndChangeQuantityConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ReturnProduct_PriceComparison",
                schema: "orders",
                table: "ReturnProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ReturnProduct_Quantity",
                schema: "orders",
                table: "ReturnProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_PriceComparison",
                schema: "orders",
                table: "Products");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ReturnProduct_Quantity",
                schema: "orders",
                table: "ReturnProducts",
                sql: "\"Quantity\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ReturnProduct_Quantity",
                schema: "orders",
                table: "ReturnProducts");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ReturnProduct_PriceComparison",
                schema: "orders",
                table: "ReturnProducts",
                sql: "\"UnitPrice\" <= \"Price\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ReturnProduct_Quantity",
                schema: "orders",
                table: "ReturnProducts",
                sql: "\"Quantity\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_PriceComparison",
                schema: "orders",
                table: "Products",
                sql: "\"UnitPrice\" <= \"Price\"");
        }
    }
}
