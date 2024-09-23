using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoPropIndexForComplaintAndReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Returns_Id_CreatedAt",
                schema: "orders",
                table: "Returns",
                columns: new[] { "Id", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Comaplaints_Id_CreatedAt",
                schema: "orders",
                table: "Comaplaints",
                columns: new[] { "Id", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Returns_Id_CreatedAt",
                schema: "orders",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Comaplaints_Id_CreatedAt",
                schema: "orders",
                table: "Comaplaints");
        }
    }
}
