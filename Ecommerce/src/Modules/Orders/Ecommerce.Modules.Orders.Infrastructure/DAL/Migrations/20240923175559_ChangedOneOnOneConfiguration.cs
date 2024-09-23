using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOneOnOneConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "orders",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                schema: "orders",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OrderId",
                schema: "orders",
                table: "Customers",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Orders_OrderId",
                schema: "orders",
                table: "Customers",
                column: "OrderId",
                principalSchema: "orders",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Orders_OrderId",
                schema: "orders",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_OrderId",
                schema: "orders",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "orders",
                table: "Customers");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "orders",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "orders",
                table: "Orders",
                column: "CustomerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                schema: "orders",
                table: "Orders",
                column: "CustomerId",
                principalSchema: "orders",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
