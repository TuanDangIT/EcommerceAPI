using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedTotalPaidSumToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPaidSum",
                schema: "orders",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_TotalPaidSum",
                schema: "orders",
                table: "Orders",
                sql: "\"TotalPaidSum\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_TotalPaidSum",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPaidSum",
                schema: "orders",
                table: "Orders");
        }
    }
}
