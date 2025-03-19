using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangePrecisionForDiscountValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "carts",
                table: "Discounts",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "carts",
                table: "Discounts",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);
        }
    }
}
