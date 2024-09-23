using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFullReturnFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFullReturn",
                schema: "orders",
                table: "Returns",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFullReturn",
                schema: "orders",
                table: "Returns");
        }
    }
}
