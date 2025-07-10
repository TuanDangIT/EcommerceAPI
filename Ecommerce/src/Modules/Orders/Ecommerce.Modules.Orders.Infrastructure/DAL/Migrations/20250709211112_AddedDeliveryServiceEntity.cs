using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedDeliveryServiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_ShippingPrice",
                schema: "orders",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingService",
                schema: "orders",
                table: "Orders",
                newName: "DeliveryService_Service");

            migrationBuilder.RenameColumn(
                name: "ShippingPrice",
                schema: "orders",
                table: "Orders",
                newName: "DeliveryService_Price");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryService_Courier",
                schema: "orders",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_DeliveryService_Price",
                schema: "orders",
                table: "Orders",
                sql: "\"DeliveryService_Price\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_DeliveryService_Price",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryService_Courier",
                schema: "orders",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryService_Service",
                schema: "orders",
                table: "Orders",
                newName: "ShippingService");

            migrationBuilder.RenameColumn(
                name: "DeliveryService_Price",
                schema: "orders",
                table: "Orders",
                newName: "ShippingPrice");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_ShippingPrice",
                schema: "orders",
                table: "Orders",
                sql: "\"ShippingPrice\" >= 0");
        }
    }
}
