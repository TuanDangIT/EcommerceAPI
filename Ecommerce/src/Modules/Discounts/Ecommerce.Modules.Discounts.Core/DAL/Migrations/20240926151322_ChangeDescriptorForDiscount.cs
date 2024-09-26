using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Discounts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDescriptorForDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "discounts",
                table: "Discounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndingDate",
                schema: "discounts",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingDate",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "discounts",
                table: "Discounts",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
