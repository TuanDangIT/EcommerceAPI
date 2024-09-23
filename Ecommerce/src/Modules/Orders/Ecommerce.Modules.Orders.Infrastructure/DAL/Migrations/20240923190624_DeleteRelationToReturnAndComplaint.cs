using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRelationToReturnAndComplaint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comaplaints_Customers_CustomerId",
                schema: "orders",
                table: "Comaplaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Customers_CustomerId",
                schema: "orders",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_CustomerId",
                schema: "orders",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Comaplaints_CustomerId",
                schema: "orders",
                table: "Comaplaints");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "orders",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "orders",
                table: "Comaplaints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "orders",
                table: "Returns",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "orders",
                table: "Comaplaints",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Returns_CustomerId",
                schema: "orders",
                table: "Returns",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comaplaints_CustomerId",
                schema: "orders",
                table: "Comaplaints",
                column: "CustomerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comaplaints_Customers_CustomerId",
                schema: "orders",
                table: "Comaplaints",
                column: "CustomerId",
                principalSchema: "orders",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Customers_CustomerId",
                schema: "orders",
                table: "Returns",
                column: "CustomerId",
                principalSchema: "orders",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
