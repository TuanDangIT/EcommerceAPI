using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationsToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Returns_OrderId",
                schema: "orders",
                table: "Returns");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrderId",
                schema: "orders",
                table: "Returns",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Returns_OrderId",
                schema: "orders",
                table: "Returns");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrderId",
                schema: "orders",
                table: "Returns",
                column: "OrderId");
        }
    }
}
