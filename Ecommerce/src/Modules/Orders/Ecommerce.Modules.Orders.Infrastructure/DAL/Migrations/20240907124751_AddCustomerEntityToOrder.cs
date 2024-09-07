using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerEntityToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "orders",
                table: "Order",
                newName: "Customer_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "Customer_Email",
                schema: "orders",
                table: "Order",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_FirstName",
                schema: "orders",
                table: "Order",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_LastName",
                schema: "orders",
                table: "Order",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_PhoneNumber",
                schema: "orders",
                table: "Order",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer_Email",
                schema: "orders",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Customer_FirstName",
                schema: "orders",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Customer_LastName",
                schema: "orders",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Customer_PhoneNumber",
                schema: "orders",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Customer_CustomerId",
                schema: "orders",
                table: "Order",
                newName: "CustomerId");
        }
    }
}
