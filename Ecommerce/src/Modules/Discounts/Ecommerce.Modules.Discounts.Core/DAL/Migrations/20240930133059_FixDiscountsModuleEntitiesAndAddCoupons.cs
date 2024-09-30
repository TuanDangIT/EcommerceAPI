using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Discounts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixDiscountsModuleEntitiesAndAddCoupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NominalValue",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "Percent",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "discounts",
                table: "Discounts",
                newName: "StripePromotionCodeId");

            migrationBuilder.RenameColumn(
                name: "EndingDate",
                schema: "discounts",
                table: "Discounts",
                newName: "ExpiresAt");

            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                schema: "discounts",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "discounts",
                table: "Discounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Redemptions",
                schema: "discounts",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "discounts",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Coupons",
                schema: "discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    StripeCouponId = table.Column<string>(type: "text", nullable: false),
                    Redemptions = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NominalValue = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: true),
                    Percent = table.Column<decimal>(type: "numeric(2,2)", precision: 2, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_CouponId",
                schema: "discounts",
                table: "Discounts",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_StripePromotionCodeId",
                schema: "discounts",
                table: "Discounts",
                column: "StripePromotionCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_StripeCouponId",
                schema: "discounts",
                table: "Coupons",
                column: "StripeCouponId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Coupons_CouponId",
                schema: "discounts",
                table: "Discounts",
                column: "CouponId",
                principalSchema: "discounts",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Coupons_CouponId",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropTable(
                name: "Coupons",
                schema: "discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_CouponId",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_StripePromotionCodeId",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "CouponId",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "Redemptions",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "StripePromotionCodeId",
                schema: "discounts",
                table: "Discounts",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                schema: "discounts",
                table: "Discounts",
                newName: "EndingDate");

            migrationBuilder.AddColumn<decimal>(
                name: "NominalValue",
                schema: "discounts",
                table: "Discounts",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percent",
                schema: "discounts",
                table: "Discounts",
                type: "numeric(2,2)",
                precision: 2,
                scale: 2,
                nullable: true);
        }
    }
}
