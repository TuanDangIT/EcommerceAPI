using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixShipmentApartmentNumberName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Shipment_AparmentNumber",
                schema: "orders",
                table: "Orders",
                newName: "Shipment_ApartmentNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Shipment_ApartmentNumber",
                schema: "orders",
                table: "Orders",
                newName: "Shipment_AparmentNumber");
        }
    }
}
