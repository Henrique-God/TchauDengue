using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TchauDengue.Migrations
{
    /// <inheritdoc />
    public partial class userPdfAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddedWhen",
                table: "PageHistory",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<string>(
                name: "PdfPublicId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PdfUrl",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PicturePublicId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PageHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfPublicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PdfUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PicturePublicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PageHistory");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "PageHistory",
                newName: "AddedWhen");
        }
    }
}
