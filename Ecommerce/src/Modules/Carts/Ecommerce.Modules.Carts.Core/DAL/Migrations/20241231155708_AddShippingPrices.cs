using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                schema: "carts",
                table: "Discounts",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Shipment_Price",
                schema: "carts",
                table: "CheckoutCarts",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shipment_Service",
                schema: "carts",
                table: "CheckoutCarts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipment_Price",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "Shipment_Service",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                schema: "carts",
                table: "Discounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16,
                oldNullable: true);
        }
    }
}
