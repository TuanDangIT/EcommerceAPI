using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Discounts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "discounts");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offers",
                newSchema: "discounts");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "Discounts",
                newSchema: "discounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Offers",
                schema: "discounts",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "Discounts",
                schema: "discounts",
                newName: "Discounts");
        }
    }
}
