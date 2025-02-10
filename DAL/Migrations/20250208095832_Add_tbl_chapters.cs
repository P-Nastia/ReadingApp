using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_tbl_chapters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_paragraphs_tbl_books_BookId",
                table: "tbl_paragraphs");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "tbl_paragraphs",
                newName: "ChapterId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_paragraphs_BookId",
                table: "tbl_paragraphs",
                newName: "IX_tbl_paragraphs_ChapterId");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "tbl_users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateTable(
                name: "tbl_chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    BookId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_chapters_tbl_books_BookId",
                        column: x => x.BookId,
                        principalTable: "tbl_books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_chapters_BookId",
                table: "tbl_chapters",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_paragraphs_tbl_chapters_ChapterId",
                table: "tbl_paragraphs",
                column: "ChapterId",
                principalTable: "tbl_chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_paragraphs_tbl_chapters_ChapterId",
                table: "tbl_paragraphs");

            migrationBuilder.DropTable(
                name: "tbl_chapters");

            migrationBuilder.RenameColumn(
                name: "ChapterId",
                table: "tbl_paragraphs",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_paragraphs_ChapterId",
                table: "tbl_paragraphs",
                newName: "IX_tbl_paragraphs_BookId");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "tbl_users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_paragraphs_tbl_books_BookId",
                table: "tbl_paragraphs",
                column: "BookId",
                principalTable: "tbl_books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
