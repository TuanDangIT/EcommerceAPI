using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Modules.Mails.Api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ExtractAttachmentFileAsNewEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentFileNames",
                schema: "mails",
                table: "Mails");

            migrationBuilder.CreateTable(
                name: "AttachmentFile",
                schema: "mails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MailId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentFile_Mails_MailId",
                        column: x => x.MailId,
                        principalSchema: "mails",
                        principalTable: "Mails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFile_MailId",
                schema: "mails",
                table: "AttachmentFile",
                column: "MailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentFile",
                schema: "mails");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentFileNames",
                schema: "mails",
                table: "Mails",
                type: "text",
                nullable: true);
        }
    }
}
