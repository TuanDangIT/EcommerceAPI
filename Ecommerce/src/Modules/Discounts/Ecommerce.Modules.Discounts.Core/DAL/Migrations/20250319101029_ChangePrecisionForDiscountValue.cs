using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Discounts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangePrecisionForDiscountValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NominalValue",
                schema: "discounts",
                table: "Coupons",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NominalValue",
                schema: "discounts",
                table: "Coupons",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
