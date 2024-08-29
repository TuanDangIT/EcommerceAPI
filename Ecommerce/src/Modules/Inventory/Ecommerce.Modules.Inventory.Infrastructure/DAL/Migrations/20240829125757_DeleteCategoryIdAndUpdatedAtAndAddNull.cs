using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCategoryIdAndUpdatedAtAndAddNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "inventory",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "inventory",
                table: "Auctions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "inventory",
                table: "Auctions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "inventory",
                table: "Auctions",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
