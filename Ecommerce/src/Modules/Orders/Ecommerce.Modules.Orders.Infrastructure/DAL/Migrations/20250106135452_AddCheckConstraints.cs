using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "ReturnProduct",
                schema: "orders");

            migrationBuilder.CreateTable(
                name: "Products",
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
                    table.PrimaryKey("PK_Products", x => new { x.OrderId, x.Id });
                    table.CheckConstraint("CK_Product_Price", "\"Price\" >= 0");
                    table.CheckConstraint("CK_Product_PriceComparison", "\"UnitPrice\" <= \"Price\"");
                    table.CheckConstraint("CK_Product_Quantity", "\"Quantity\" > 0");
                    table.CheckConstraint("CK_Product_UnitPrice", "\"UnitPrice\" >= 0");
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
                    table.PrimaryKey("PK_ReturnProducts", x => new { x.ReturnId, x.Id });
                    table.CheckConstraint("CK_ReturnProduct_Price", "\"Price\" >= 0");
                    table.CheckConstraint("CK_ReturnProduct_Quantity", "\"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_ReturnProducts_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalSchema: "orders",
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_DiscountValue",
                schema: "orders",
                table: "Orders",
                sql: "\"Discount_Value\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_ShippingPrice",
                schema: "orders",
                table: "Orders",
                sql: "\"ShippingPrice\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_TotalSum",
                schema: "orders",
                table: "Orders",
                sql: "\"TotalSum\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Complaint_RefundAmount",
                schema: "orders",
                table: "Complaints",
                sql: "\"Decision_RefundAmount\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "ReturnProducts",
                schema: "orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_DiscountValue",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_ShippingPrice",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_TotalSum",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Complaint_RefundAmount",
                schema: "orders",
                table: "Complaints");

            migrationBuilder.CreateTable(
                name: "Product",
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
    }
}
