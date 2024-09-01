using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddApartmentNumberPropertyToShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Shipment_StreetNumber",
                table: "CheckoutCarts",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<string>(
                name: "Shipment_AparmentNumber",
                table: "CheckoutCarts",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipment_AparmentNumber",
                table: "CheckoutCarts");

            migrationBuilder.AlterColumn<int>(
                name: "Shipment_StreetNumber",
                table: "CheckoutCarts",
                type: "integer",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);
        }
    }
}
