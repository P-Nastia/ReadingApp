using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_tbl_books : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "tbl_paragraphs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PdfData = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_books", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_paragraphs_BookId",
                table: "tbl_paragraphs",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_paragraphs_tbl_books_BookId",
                table: "tbl_paragraphs",
                column: "BookId",
                principalTable: "tbl_books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_paragraphs_tbl_books_BookId",
                table: "tbl_paragraphs");

            migrationBuilder.DropTable(
                name: "tbl_books");

            migrationBuilder.DropIndex(
                name: "IX_tbl_paragraphs_BookId",
                table: "tbl_paragraphs");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "tbl_paragraphs");
        }
    }
}
