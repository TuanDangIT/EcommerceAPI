using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Product",
                newSchema: "orders");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Order",
                newSchema: "orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Product",
                schema: "orders",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "Order",
                schema: "orders",
                newName: "Order");
        }
    }
}
