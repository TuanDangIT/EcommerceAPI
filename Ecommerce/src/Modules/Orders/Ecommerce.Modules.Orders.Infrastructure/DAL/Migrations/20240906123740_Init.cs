using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Shipment_City = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Shipment_PostalCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Shipment_StreetName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Shipment_StreetNumber = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Shipment_AparmentNumber = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Shipment_ReceiverFullName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Payment = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    OrderPlacedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_Product_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
