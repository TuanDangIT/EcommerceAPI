using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrectComplaintName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comaplaints_Orders_OrderId",
                schema: "orders",
                table: "Comaplaints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comaplaints",
                schema: "orders",
                table: "Comaplaints");

            migrationBuilder.RenameTable(
                name: "Comaplaints",
                schema: "orders",
                newName: "Complaints",
                newSchema: "orders");

            migrationBuilder.RenameIndex(
                name: "IX_Comaplaints_OrderId",
                schema: "orders",
                table: "Complaints",
                newName: "IX_Complaints_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Comaplaints_Id_CreatedAt",
                schema: "orders",
                table: "Complaints",
                newName: "IX_Complaints_Id_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Complaints",
                schema: "orders",
                table: "Complaints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Orders_OrderId",
                schema: "orders",
                table: "Complaints",
                column: "OrderId",
                principalSchema: "orders",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Orders_OrderId",
                schema: "orders",
                table: "Complaints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Complaints",
                schema: "orders",
                table: "Complaints");

            migrationBuilder.RenameTable(
                name: "Complaints",
                schema: "orders",
                newName: "Comaplaints",
                newSchema: "orders");

            migrationBuilder.RenameIndex(
                name: "IX_Complaints_OrderId",
                schema: "orders",
                table: "Comaplaints",
                newName: "IX_Comaplaints_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Complaints_Id_CreatedAt",
                schema: "orders",
                table: "Comaplaints",
                newName: "IX_Comaplaints_Id_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comaplaints",
                schema: "orders",
                table: "Comaplaints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comaplaints_Orders_OrderId",
                schema: "orders",
                table: "Comaplaints",
                column: "OrderId",
                principalSchema: "orders",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
