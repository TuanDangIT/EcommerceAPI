using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRefundedAmountName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decision_RefundedAmount",
                schema: "orders",
                table: "Complaints");

            migrationBuilder.AddColumn<decimal>(
                name: "Decision_RefundAmount",
                schema: "orders",
                table: "Complaints",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decision_RefundAmount",
                schema: "orders",
                table: "Complaints");

            migrationBuilder.AddColumn<decimal>(
                name: "Decision_RefundedAmount",
                schema: "orders",
                table: "Complaints",
                type: "numeric",
                nullable: true);
        }
    }
}
