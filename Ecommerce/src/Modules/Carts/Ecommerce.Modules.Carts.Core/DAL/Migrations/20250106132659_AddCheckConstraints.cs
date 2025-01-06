using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Price",
                schema: "carts",
                table: "Products",
                sql: "\"Price\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "carts",
                table: "Products",
                sql: "\"Quantity\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discount_ExpiresAt",
                schema: "carts",
                table: "Discounts",
                sql: "\"ExpiresAt\" > NOW()");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discount_Value",
                schema: "carts",
                table: "Discounts",
                sql: "\"Value\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CheckoutCart_TotalSum",
                schema: "carts",
                table: "CheckoutCarts",
                sql: "\"TotalSum\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Cart_TotalSum",
                schema: "carts",
                table: "Carts",
                sql: "\"TotalSum\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CartProduct_DiscountedPrice",
                schema: "carts",
                table: "CartProducts",
                sql: "\"DiscountedPrice\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CartProduct_Price",
                schema: "carts",
                table: "CartProducts",
                sql: "\"Price\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CartProduct_Quantity",
                schema: "carts",
                table: "CartProducts",
                sql: "\"Quantity\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Price",
                schema: "carts",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "carts",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Discount_ExpiresAt",
                schema: "carts",
                table: "Discounts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Discount_Value",
                schema: "carts",
                table: "Discounts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CheckoutCart_TotalSum",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Cart_TotalSum",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CartProduct_DiscountedPrice",
                schema: "carts",
                table: "CartProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CartProduct_Price",
                schema: "carts",
                table: "CartProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CartProduct_Quantity",
                schema: "carts",
                table: "CartProducts");
        }
    }
}
