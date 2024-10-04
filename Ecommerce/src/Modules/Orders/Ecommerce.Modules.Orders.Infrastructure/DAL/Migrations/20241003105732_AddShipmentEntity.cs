using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipment_ApartmentNumber",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Shipment_City",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Shipment_PostalCode",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Shipment_StreetName",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Shipment_StreetNumber",
                schema: "orders",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "Shipments",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackingNumber = table.Column<string>(type: "text", nullable: true),
                    LabelId = table.Column<string>(type: "text", nullable: true),
                    Receiver_FirstName = table.Column<string>(type: "text", nullable: false),
                    Receiver_LastName = table.Column<string>(type: "text", nullable: false),
                    Receiver_Email = table.Column<string>(type: "text", nullable: false),
                    Receiver_Phone = table.Column<string>(type: "text", nullable: false),
                    Receiver_CompanyName = table.Column<string>(type: "text", nullable: true),
                    Receiver_Address_Street = table.Column<string>(type: "text", nullable: false),
                    Receiver_Address_BuildingNumber = table.Column<string>(type: "text", nullable: false),
                    Receiver_Address_City = table.Column<string>(type: "text", nullable: false),
                    Receiver_Address_PostCode = table.Column<string>(type: "text", nullable: false),
                    Receiver_Address_CountryCode = table.Column<string>(type: "text", nullable: false),
                    Insurance_Amount = table.Column<string>(type: "text", nullable: true),
                    Insurance_Currency = table.Column<string>(type: "text", nullable: true),
                    Service = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcel",
                schema: "orders",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Dimensions_Length = table.Column<string>(type: "text", nullable: false),
                    Dimensions_Width = table.Column<string>(type: "text", nullable: false),
                    Dimensions_Height = table.Column<string>(type: "text", nullable: false),
                    Dimensions_Unit = table.Column<string>(type: "text", nullable: false),
                    Weight_Amount = table.Column<string>(type: "text", nullable: false),
                    Weight_Unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcel", x => new { x.ShipmentId, x.Id });
                    table.ForeignKey(
                        name: "FK_Parcel_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalSchema: "orders",
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                schema: "orders",
                table: "Shipments",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcel",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Shipments",
                schema: "orders");

            migrationBuilder.AddColumn<string>(
                name: "Shipment_ApartmentNumber",
                schema: "orders",
                table: "Orders",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Shipment_City",
                schema: "orders",
                table: "Orders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Shipment_PostalCode",
                schema: "orders",
                table: "Orders",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Shipment_StreetName",
                schema: "orders",
                table: "Orders",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Shipment_StreetNumber",
                schema: "orders",
                table: "Orders",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }
    }
}
