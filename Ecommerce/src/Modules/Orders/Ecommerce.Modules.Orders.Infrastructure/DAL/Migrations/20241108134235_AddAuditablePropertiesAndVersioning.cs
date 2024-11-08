using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditablePropertiesAndVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPlacedAt",
                schema: "orders",
                table: "Orders",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Id_OrderPlacedAt",
                schema: "orders",
                table: "Orders",
                newName: "IX_Orders_Id_CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "orders",
                table: "Shipments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "orders",
                table: "Returns",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "orders",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "orders",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "orders",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "orders",
                table: "Complaints",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "orders",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "orders",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "orders",
                table: "Orders");

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

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "orders",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "orders",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "orders",
                table: "Complaints");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "orders",
                table: "Orders",
                newName: "OrderPlacedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Id_CreatedAt",
                schema: "orders",
                table: "Orders",
                newName: "IX_Orders_Id_OrderPlacedAt");
        }
    }
}
