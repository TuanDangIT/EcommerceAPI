using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixQuantityCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Auction_Quantity",
                schema: "inventory",
                table: "Auctions");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Auction_Quantity",
                schema: "inventory",
                table: "Auctions",
                sql: "\"Quantity\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Auction_Quantity",
                schema: "inventory",
                table: "Auctions");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Auction_Quantity",
                schema: "inventory",
                table: "Auctions",
                sql: "\"Quantity\" > 0");
        }
    }
}
