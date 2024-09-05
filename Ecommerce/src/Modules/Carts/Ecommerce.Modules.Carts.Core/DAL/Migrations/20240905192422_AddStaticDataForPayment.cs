using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStaticDataForPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "carts",
                table: "Payments",
                columns: new[] { "Id", "PaymentMethod" },
                values: new object[] { new Guid("db7346d0-b93e-402a-8025-75b393434c26"), "card" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "carts",
                table: "Payments",
                keyColumn: "Id",
                keyValue: new Guid("db7346d0-b93e-402a-8025-75b393434c26"));
        }
    }
}
