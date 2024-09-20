using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintAndReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer_Email",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Customer_FirstName",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Customer_LastName",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Customer_PhoneNumber",
                schema: "orders",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Customer_CustomerId",
                schema: "orders",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "orders",
                table: "Products",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "orders",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "orders",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirstName = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    LastName = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comaplaints",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AdditionalNote = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comaplaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comaplaints_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "orders",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comaplaints_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReasonForReturn = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AdditionalNote = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returns_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "orders",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Returns_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintProducts",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComplaintId = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ReturnProducts",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnId = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnProducts", x => new { x.ReturnId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReturnProducts_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalSchema: "orders",
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrderStatus",
                value: "ParcelPacked");

            migrationBuilder.UpdateData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "OrderStatus",
                value: "Shipped");

            migrationBuilder.UpdateData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "OrderStatus",
                value: "Completed");

            migrationBuilder.InsertData(
                schema: "orders",
                table: "Statuses",
                columns: new[] { "Id", "OrderStatus" },
                values: new object[,]
                {
                    { 5, "Cancelled" },
                    { 6, "Returned" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "orders",
                table: "Orders",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comaplaints_CustomerId",
                schema: "orders",
                table: "Comaplaints",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comaplaints_OrderId",
                schema: "orders",
                table: "Comaplaints",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_CustomerId",
                schema: "orders",
                table: "Returns",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrderId",
                schema: "orders",
                table: "Returns",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                schema: "orders",
                table: "Orders",
                column: "CustomerId",
                principalSchema: "orders",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ComplaintProducts",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "ReturnProducts",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Comaplaints",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Returns",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DeleteData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "orders",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "orders",
                table: "Orders",
                newName: "Customer_CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "orders",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<Guid>(
                name: "Customer_CustomerId",
                schema: "orders",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Customer_Email",
                schema: "orders",
                table: "Orders",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_FirstName",
                schema: "orders",
                table: "Orders",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_LastName",
                schema: "orders",
                table: "Orders",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Customer_PhoneNumber",
                schema: "orders",
                table: "Orders",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrderStatus",
                value: "Shipped");

            migrationBuilder.UpdateData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "OrderStatus",
                value: "Completed");

            migrationBuilder.UpdateData(
                schema: "orders",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "OrderStatus",
                value: "Cancelled");
        }
    }
}
