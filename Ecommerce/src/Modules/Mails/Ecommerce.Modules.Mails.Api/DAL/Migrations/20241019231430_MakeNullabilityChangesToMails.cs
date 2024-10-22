using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Mails.Api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeNullabilityChangesToMails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Customers_CustomerId",
                schema: "mails",
                table: "Mails");

            migrationBuilder.DropColumn(
                name: "PdfUrlPath",
                schema: "mails",
                table: "Mails");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "mails",
                table: "Mails",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "mails",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "mails",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Customers_CustomerId",
                schema: "mails",
                table: "Mails",
                column: "CustomerId",
                principalSchema: "mails",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Customers_CustomerId",
                schema: "mails",
                table: "Mails");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "mails",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "mails",
                table: "Customers");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "mails",
                table: "Mails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PdfUrlPath",
                schema: "mails",
                table: "Mails",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Customers_CustomerId",
                schema: "mails",
                table: "Mails",
                column: "CustomerId",
                principalSchema: "mails",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
