using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ExpandDiscountLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "ReturnProducts",
                schema: "orders");

            migrationBuilder.DropColumn(
                name: "DiscountCode",
                schema: "orders",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Discount_Code",
                schema: "orders",
                table: "Orders",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discount_SKU",
                schema: "orders",
                table: "Orders",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Discount_Type",
                schema: "orders",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount_Value",
                schema: "orders",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingPrice",
                schema: "orders",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ShippingService",
                schema: "orders",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_Product_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnProduct",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnId = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnProduct", x => new { x.ReturnId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReturnProduct_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalSchema: "orders",
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "ReturnProduct",
                schema: "orders");

            migrationBuilder.DropColumn(
                name: "Discount_Code",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount_SKU",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount_Type",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount_Value",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingPrice",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingService",
                schema: "orders",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                schema: "orders",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_Products_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnProducts",
                schema: "orders",
                columns: table => new
                {
                    ReturnId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnProducts", x => new { x.ReturnId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReturnProducts_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalSchema: "orders",
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
