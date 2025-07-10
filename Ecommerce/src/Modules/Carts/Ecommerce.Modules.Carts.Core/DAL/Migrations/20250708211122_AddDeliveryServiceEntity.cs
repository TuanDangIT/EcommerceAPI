using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryServiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipment_Price",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "Shipment_Service",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.AddColumn<int>(
                name: "Shipment_DeliveryServiceId",
                schema: "carts",
                table: "CheckoutCarts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryServices",
                schema: "carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Courier = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryServices", x => x.Id);
                    table.CheckConstraint("CK_DeliveryService_Price", "\"Price\" >= 0");
                });

            migrationBuilder.InsertData(
                schema: "carts",
                table: "DeliveryServices",
                columns: new[] { "Id", "Courier", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 0, true, "Kurier InPost", 3m },
                    { 2, 1, true, "Kurier DPD", 3.5m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutCarts_Shipment_DeliveryServiceId",
                schema: "carts",
                table: "CheckoutCarts",
                column: "Shipment_DeliveryServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutCarts_DeliveryServices_Shipment_DeliveryServiceId",
                schema: "carts",
                table: "CheckoutCarts",
                column: "Shipment_DeliveryServiceId",
                principalSchema: "carts",
                principalTable: "DeliveryServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutCarts_DeliveryServices_Shipment_DeliveryServiceId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropTable(
                name: "DeliveryServices",
                schema: "carts");

            migrationBuilder.DropIndex(
                name: "IX_CheckoutCarts_Shipment_DeliveryServiceId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "Shipment_DeliveryServiceId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.AddColumn<decimal>(
                name: "Shipment_Price",
                schema: "carts",
                table: "CheckoutCarts",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shipment_Service",
                schema: "carts",
                table: "CheckoutCarts",
                type: "text",
                nullable: true);
        }
    }
}
