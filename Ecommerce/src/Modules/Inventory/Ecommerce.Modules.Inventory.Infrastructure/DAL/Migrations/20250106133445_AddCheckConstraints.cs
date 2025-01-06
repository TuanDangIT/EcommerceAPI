using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_Grade",
                schema: "inventory",
                table: "Reviews",
                sql: "\"Grade\" >= 1 AND \"Grade\" <= 10");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Price",
                schema: "inventory",
                table: "Products",
                sql: "\"Price\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "inventory",
                table: "Products",
                sql: "\"Quantity\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Reserved",
                schema: "inventory",
                table: "Products",
                sql: "\"Reserved\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_VAT",
                schema: "inventory",
                table: "Products",
                sql: "\"VAT\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Auction_Price",
                schema: "inventory",
                table: "Auctions",
                sql: "\"Price\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Auction_Quantity",
                schema: "inventory",
                table: "Auctions",
                sql: "\"Quantity\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_Grade",
                schema: "inventory",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Price",
                schema: "inventory",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "inventory",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Reserved",
                schema: "inventory",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_VAT",
                schema: "inventory",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Auction_Price",
                schema: "inventory",
                table: "Auctions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Auction_Quantity",
                schema: "inventory",
                table: "Auctions");
        }
    }
}
