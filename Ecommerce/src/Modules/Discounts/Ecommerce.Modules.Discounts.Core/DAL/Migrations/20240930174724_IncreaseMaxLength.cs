using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Discounts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseMaxLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "discounts",
                table: "Discounts",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "discounts",
                table: "Coupons",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "discounts",
                table: "Discounts",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "discounts",
                table: "Coupons",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);
        }
    }
}
