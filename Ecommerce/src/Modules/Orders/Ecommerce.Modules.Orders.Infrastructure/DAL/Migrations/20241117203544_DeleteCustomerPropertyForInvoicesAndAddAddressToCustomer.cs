using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCustomerPropertyForInvoicesAndAddAddressToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shipments_OrderId",
                schema: "orders",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Customer_Email",
                schema: "orders",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Customer_FirstName",
                schema: "orders",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Customer_LastName",
                schema: "orders",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Customer_PhoneNumber",
                schema: "orders",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "Address_BuildingNumber",
                schema: "orders",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "orders",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_CountryCode",
                schema: "orders",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostCode",
                schema: "orders",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                schema: "orders",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                schema: "orders",
                table: "Shipments",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shipments_OrderId",
                schema: "orders",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Address_BuildingNumber",
                schema: "orders",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "orders",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Address_CountryCode",
                schema: "orders",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Address_PostCode",
                schema: "orders",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                schema: "orders",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Customer_Email",
                schema: "orders",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_FirstName",
                schema: "orders",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_LastName",
                schema: "orders",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_PhoneNumber",
                schema: "orders",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                schema: "orders",
                table: "Shipments",
                column: "OrderId",
                unique: true);
        }
    }
}
