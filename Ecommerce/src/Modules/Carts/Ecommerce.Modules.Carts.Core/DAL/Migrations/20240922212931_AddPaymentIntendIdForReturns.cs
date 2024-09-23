using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentIntendIdForReturns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntendId",
                schema: "carts",
                table: "CheckoutCarts",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                schema: "carts",
                table: "CartProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripePaymentIntendId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                schema: "carts",
                table: "CartProducts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
