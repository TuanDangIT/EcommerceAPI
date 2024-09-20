using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeComplaintImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplaintProducts",
                schema: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                schema: "orders",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Decision_AdditionalInformation",
                schema: "orders",
                table: "Comaplaints",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Decision_DecisionText",
                schema: "orders",
                table: "Comaplaints",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decision_AdditionalInformation",
                schema: "orders",
                table: "Comaplaints");

            migrationBuilder.DropColumn(
                name: "Decision_DecisionText",
                schema: "orders",
                table: "Comaplaints");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                schema: "orders",
                table: "Products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "ComplaintProducts",
                schema: "orders",
                columns: table => new
                {
                    ComplaintId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintProducts", x => new { x.ComplaintId, x.Id });
                    table.ForeignKey(
                        name: "FK_ComplaintProducts_Comaplaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalSchema: "orders",
                        principalTable: "Comaplaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
