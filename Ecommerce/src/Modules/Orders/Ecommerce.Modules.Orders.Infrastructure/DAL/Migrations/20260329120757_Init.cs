using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalSum = table.Column<decimal>(type: "numeric", nullable: false),
                    Payment = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ClientAdditionalInformation = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CompanyAdditionalInformation = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Discount_Type = table.Column<string>(type: "text", nullable: true),
                    Discount_Code = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: true),
                    Discount_Value = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: true),
                    Discount_SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    StripePaymentIntentId = table.Column<string>(type: "text", nullable: false),
                    DeliveryService_Courier = table.Column<string>(type: "text", nullable: false),
                    DeliveryService_Service = table.Column<string>(type: "text", nullable: false),
                    DeliveryService_Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CK_Order_DeliveryService_Price", "\"DeliveryService_Price\" >= 0");
                    table.CheckConstraint("CK_Order_DiscountValue", "\"Discount_Value\" > 0");
                    table.CheckConstraint("CK_Order_TotalSum", "\"TotalSum\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.CheckConstraint("CK_Product_Price", "\"Price\" >= 0");
                    table.CheckConstraint("CK_Product_Quantity", "\"Quantity\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AdditionalNote = table.Column<string>(type: "text", nullable: true),
                    Decision_DecisionText = table.Column<string>(type: "text", nullable: true),
                    Decision_AdditionalInformation = table.Column<string>(type: "text", nullable: true),
                    Decision_RefundAmount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.CheckConstraint("CK_Complaint_RefundAmount", "\"Decision_RefundAmount\" >= 0");
                    table.ForeignKey(
                        name: "FK_Complaints_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirstName = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    LastName = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Address_Street = table.Column<string>(type: "text", nullable: false),
                    Address_BuildingNumber = table.Column<string>(type: "text", nullable: false),
                    Address_City = table.Column<string>(type: "text", nullable: false),
                    Address_PostCode = table.Column<string>(type: "text", nullable: false),
                    Address_CountryCode = table.Column<string>(type: "text", nullable: false),
                    Address_Country = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceNo = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.CheckConstraint("CK_OrderItem_Price", "\"Price\" >= 0");
                    table.CheckConstraint("CK_OrderItem_Quantity", "\"Quantity\" >= 0");
                    table.CheckConstraint("CK_OrderItem_UnitPrice", "\"UnitPrice\" >= 0");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
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
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReasonForReturn = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AdditionalNote = table.Column<string>(type: "text", nullable: true),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    IsFullReturn = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returns_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackingNumber = table.Column<string>(type: "text", nullable: true),
                    LabelId = table.Column<string>(type: "text", nullable: true),
                    LabelCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    Receiver_Address_Country = table.Column<string>(type: "text", nullable: false),
                    Insurance_Amount = table.Column<string>(type: "text", nullable: true),
                    Insurance_Currency = table.Column<string>(type: "text", nullable: true),
                    Service = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
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
                name: "ReturnProducts",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnId = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ImagePathUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnProducts", x => new { x.ReturnId, x.Id });
                    table.CheckConstraint("CK_ReturnProduct_Price", "\"Price\" >= 0");
                    table.CheckConstraint("CK_ReturnProduct_Quantity", "\"Quantity\" >= 0");
                    table.CheckConstraint("CK_ReturnProduct_UnitPrice", "\"UnitPrice\" >= 0");
                    table.ForeignKey(
                        name: "FK_ReturnProducts_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalSchema: "orders",
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                schema: "orders",
                table: "Statuses",
                columns: new[] { "Id", "OrderStatus" },
                values: new object[,]
                {
                    { 1, "Placed" },
                    { 2, "ParcelPacked" },
                    { 3, "Shipped" },
                    { 4, "Completed" },
                    { 5, "Cancelled" },
                    { 6, "Returned" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_Id_CreatedAt",
                schema: "orders",
                table: "Complaints",
                columns: new[] { "Id", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_OrderId",
                schema: "orders",
                table: "Complaints",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OrderId",
                schema: "orders",
                table: "Customers",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                schema: "orders",
                table: "Invoices",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                schema: "orders",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id_CreatedAt",
                schema: "orders",
                table: "Orders",
                columns: new[] { "Id", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                schema: "orders",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returns_Id_CreatedAt",
                schema: "orders",
                table: "Returns",
                columns: new[] { "Id", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrderId",
                schema: "orders",
                table: "Returns",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                schema: "orders",
                table: "Shipments",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complaints",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "OrderItems",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Parcels",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "ReturnProducts",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Statuses",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Shipments",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Returns",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "orders");
        }
    }
}
