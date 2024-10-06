using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StripePromotionCodeId",
                schema: "carts",
                table: "Discounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "carts",
                table: "Discounts",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "carts",
                table: "Discounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                schema: "carts",
                table: "Discounts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "carts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "SKU",
                schema: "carts",
                table: "Discounts");

            migrationBuilder.AlterColumn<string>(
                name: "StripePromotionCodeId",
                schema: "carts",
                table: "Discounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "carts",
                table: "Discounts",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48);
        }
    }
}
