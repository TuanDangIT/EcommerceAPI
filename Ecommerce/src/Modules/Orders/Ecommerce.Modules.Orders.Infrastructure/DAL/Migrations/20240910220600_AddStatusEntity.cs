using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Order_OrderId",
                schema: "orders",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                schema: "orders",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                schema: "orders",
                newName: "Orders",
                newSchema: "orders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                schema: "orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Statuses",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "orders",
                table: "Statuses",
                columns: new[] { "Id", "OrderStatus" },
                values: new object[,]
                {
                    { 1, "Placed" },
                    { 2, "Shipped" },
                    { 3, "Completed" },
                    { 4, "Cancelled" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Orders_OrderId",
                schema: "orders",
                table: "Product",
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
                name: "FK_Product_Orders_OrderId",
                schema: "orders",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Statuses",
                schema: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                schema: "orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "orders",
                newName: "Order",
                newSchema: "orders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                schema: "orders",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Order_OrderId",
                schema: "orders",
                table: "Product",
                column: "OrderId",
                principalSchema: "orders",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
