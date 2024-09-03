using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "carts");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "carts");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payments",
                newSchema: "carts");

            migrationBuilder.RenameTable(
                name: "CheckoutCarts",
                newName: "CheckoutCarts",
                newSchema: "carts");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Carts",
                newSchema: "carts");

            migrationBuilder.RenameTable(
                name: "CartProducts",
                newName: "CartProducts",
                newSchema: "carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Products",
                schema: "carts",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "carts",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "CheckoutCarts",
                schema: "carts",
                newName: "CheckoutCarts");

            migrationBuilder.RenameTable(
                name: "Carts",
                schema: "carts",
                newName: "Carts");

            migrationBuilder.RenameTable(
                name: "CartProducts",
                schema: "carts",
                newName: "CartProducts");
        }
    }
}
