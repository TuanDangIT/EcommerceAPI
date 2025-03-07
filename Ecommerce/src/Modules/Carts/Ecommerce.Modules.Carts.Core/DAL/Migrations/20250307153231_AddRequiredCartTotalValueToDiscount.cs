using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredCartTotalValueToDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RequiredCartTotalValue",
                schema: "carts",
                table: "Discounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discount_RequiredCartTotalValue",
                schema: "carts",
                table: "Discounts",
                sql: "\"RequiredCartTotalValue\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Discount_RequiredCartTotalValue",
                schema: "carts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "RequiredCartTotalValue",
                schema: "carts",
                table: "Discounts");
        }
    }
}
