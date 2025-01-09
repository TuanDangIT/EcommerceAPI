using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixCheckConstraintForQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "carts",
                table: "Products");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "carts",
                table: "Products",
                sql: "\"Quantity\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "carts",
                table: "Products");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "carts",
                table: "Products",
                sql: "\"Quantity\" > 0");
        }
    }
}
