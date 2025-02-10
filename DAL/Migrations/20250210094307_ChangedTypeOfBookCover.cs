using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTypeOfBookCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "tbl_books");

            migrationBuilder.AddColumn<string>(
                name: "CoverURL",
                table: "tbl_books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverURL",
                table: "tbl_books");

            migrationBuilder.AddColumn<byte[]>(
                name: "Cover",
                table: "tbl_books",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
