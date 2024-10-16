using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyAdditionalInformationPropertyToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcel",
                schema: "orders");

            migrationBuilder.RenameColumn(
                name: "AdditionalInformation",
                schema: "orders",
                table: "Orders",
                newName: "CompanyAdditionalInformation");

            migrationBuilder.AddColumn<DateTime>(
                name: "LabelCreatedAt",
                schema: "orders",
                table: "Shipments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientAdditionalInformation",
                schema: "orders",
                table: "Orders",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Parcels",
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
                    table.PrimaryKey("PK_Parcels", x => new { x.ShipmentId, x.Id });
                    table.ForeignKey(
                        name: "FK_Parcels_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalSchema: "orders",
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcels",
                schema: "orders");

            migrationBuilder.DropColumn(
                name: "LabelCreatedAt",
                schema: "orders",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ClientAdditionalInformation",
                schema: "orders",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CompanyAdditionalInformation",
                schema: "orders",
                table: "Orders",
                newName: "AdditionalInformation");

            migrationBuilder.CreateTable(
                name: "Parcel",
                schema: "orders",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Dimensions_Height = table.Column<string>(type: "text", nullable: false),
                    Dimensions_Length = table.Column<string>(type: "text", nullable: false),
                    Dimensions_Unit = table.Column<string>(type: "text", nullable: false),
                    Dimensions_Width = table.Column<string>(type: "text", nullable: false),
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
        }
    }
}
