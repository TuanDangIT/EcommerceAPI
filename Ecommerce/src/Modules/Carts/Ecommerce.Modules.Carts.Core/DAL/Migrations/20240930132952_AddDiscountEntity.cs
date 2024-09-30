using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                schema: "carts",
                table: "CheckoutCarts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discounts",
                schema: "carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    StripePromotionCodeId = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutCarts_DiscountId",
                schema: "carts",
                table: "CheckoutCarts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Code",
                schema: "carts",
                table: "Discounts",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutCarts_Discounts_DiscountId",
                schema: "carts",
                table: "CheckoutCarts",
                column: "DiscountId",
                principalSchema: "carts",
                principalTable: "Discounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutCarts_Discounts_DiscountId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropTable(
                name: "Discounts",
                schema: "carts");

            migrationBuilder.DropIndex(
                name: "IX_CheckoutCarts_DiscountId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                schema: "carts",
                table: "CheckoutCarts");
        }
    }
}
