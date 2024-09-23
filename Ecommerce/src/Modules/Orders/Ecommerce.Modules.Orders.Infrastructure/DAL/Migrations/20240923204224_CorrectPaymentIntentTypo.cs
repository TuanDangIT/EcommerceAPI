using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrectPaymentIntentTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripePaymentIntendId",
                schema: "orders",
                table: "Orders",
                newName: "StripePaymentIntentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripePaymentIntentId",
                schema: "orders",
                table: "Orders",
                newName: "StripePaymentIntendId");
        }
    }
}
