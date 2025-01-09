using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationBetweenEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_CheckoutCarts_CheckoutCartId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CheckoutCartId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CheckoutCartId",
                schema: "carts",
                table: "Carts");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                schema: "carts",
                table: "CheckoutCarts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutCarts_CartId",
                schema: "carts",
                table: "CheckoutCarts",
                column: "CartId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutCarts_Carts_CartId",
                schema: "carts",
                table: "CheckoutCarts",
                column: "CartId",
                principalSchema: "carts",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutCarts_Carts_CartId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropIndex(
                name: "IX_CheckoutCarts_CartId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "CartId",
                schema: "carts",
                table: "CheckoutCarts");

            migrationBuilder.AddColumn<Guid>(
                name: "CheckoutCartId",
                schema: "carts",
                table: "Carts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CheckoutCartId",
                schema: "carts",
                table: "Carts",
                column: "CheckoutCartId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_CheckoutCarts_CheckoutCartId",
                schema: "carts",
                table: "Carts",
                column: "CheckoutCartId",
                principalSchema: "carts",
                principalTable: "CheckoutCarts",
                principalColumn: "Id");
        }
    }
}
