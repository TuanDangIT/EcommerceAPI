using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditablePropertiesAndVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "inventory",
                table: "ProductParameters");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "inventory",
                table: "ProductParameters");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "inventory",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "inventory",
                table: "Parameters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "inventory",
                table: "Manufacturers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "inventory",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "inventory",
                table: "Auctions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "inventory",
                table: "Auctions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "inventory",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "inventory",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "inventory",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "inventory",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "inventory",
                table: "Auctions");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "inventory",
                table: "ProductParameters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "inventory",
                table: "ProductParameters",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
