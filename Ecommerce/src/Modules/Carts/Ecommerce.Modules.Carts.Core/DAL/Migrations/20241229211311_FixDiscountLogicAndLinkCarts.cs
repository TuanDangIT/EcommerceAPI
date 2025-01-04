using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixDiscountLogicAndLinkCarts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "carts",
                table: "Carts",
                newName: "CheckoutCartId");

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                schema: "carts",
                table: "Carts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSum",
                schema: "carts",
                table: "Carts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CheckoutCartId",
                schema: "carts",
                table: "Carts",
                column: "CheckoutCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_DiscountId",
                schema: "carts",
                table: "Carts",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_CheckoutCarts_CheckoutCartId",
                schema: "carts",
                table: "Carts",
                column: "CheckoutCartId",
                principalSchema: "carts",
                principalTable: "CheckoutCarts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Discounts_DiscountId",
                schema: "carts",
                table: "Carts",
                column: "DiscountId",
                principalSchema: "carts",
                principalTable: "Discounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_CheckoutCarts_CheckoutCartId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Discounts_DiscountId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CheckoutCartId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_DiscountId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "TotalSum",
                schema: "carts",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "CheckoutCartId",
                schema: "carts",
                table: "Carts",
                newName: "CustomerId");
        }
    }
}
