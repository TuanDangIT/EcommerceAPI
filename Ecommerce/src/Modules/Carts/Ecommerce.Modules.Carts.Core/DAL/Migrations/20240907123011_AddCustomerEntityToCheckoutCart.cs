using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerEntityToCheckoutCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipment_ReceiverFullName",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "carts",
                table: "CheckoutCarts",
                newName: "Customer_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                schema: "carts",
                table: "CheckoutCarts",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_Email",
                schema: "carts",
                table: "CheckoutCarts",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_FirstName",
                schema: "carts",
                table: "CheckoutCarts",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_LastName",
                schema: "carts",
                table: "CheckoutCarts",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_PhoneNumber",
                schema: "carts",
                table: "CheckoutCarts",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "Customer_Email",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "Customer_FirstName",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "Customer_LastName",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "Customer_PhoneNumber",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.RenameColumn(
                name: "Customer_CustomerId",
                schema: "carts",
                table: "CheckoutCarts",
                newName: "CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "Shipment_ReceiverFullName",
                schema: "carts",
                table: "CheckoutCarts",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);
        }
    }
}
