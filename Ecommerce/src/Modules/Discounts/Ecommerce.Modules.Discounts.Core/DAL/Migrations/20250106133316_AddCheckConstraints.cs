using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Discounts.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_ExpiresAt",
                schema: "discounts",
                table: "Offers",
                sql: "\"ExpiresAt\" > NOW()");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_OfferedPrice",
                schema: "discounts",
                table: "Offers",
                sql: "\"OfferedPrice\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_OldPrice",
                schema: "discounts",
                table: "Offers",
                sql: "\"OldPrice\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_PriceComparison",
                schema: "discounts",
                table: "Offers",
                sql: "\"OldPrice\" > \"OfferedPrice\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discount_ExpiresAt",
                schema: "discounts",
                table: "Discounts",
                sql: "\"ExpiresAt\" > NOW()");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discount_Redemptions",
                schema: "discounts",
                table: "Discounts",
                sql: "\"Redemptions\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coupon_NominalValue",
                schema: "discounts",
                table: "Coupons",
                sql: "\"NominalValue\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coupon_Percent",
                schema: "discounts",
                table: "Coupons",
                sql: "\"Percent\" >= 0.01 AND \"Percent\" <= 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_ExpiresAt",
                schema: "discounts",
                table: "Offers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_OfferedPrice",
                schema: "discounts",
                table: "Offers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_OldPrice",
                schema: "discounts",
                table: "Offers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_PriceComparison",
                schema: "discounts",
                table: "Offers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Discount_ExpiresAt",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Discount_Redemptions",
                schema: "discounts",
                table: "Discounts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Coupon_NominalValue",
                schema: "discounts",
                table: "Coupons");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Coupon_Percent",
                schema: "discounts",
                table: "Coupons");
        }
    }
}
