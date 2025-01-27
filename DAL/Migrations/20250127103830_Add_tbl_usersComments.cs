using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_tbl_usersComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_usersComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    ParagraphId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_usersComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_usersComments_tbl_paragraphs_ParagraphId",
                        column: x => x.ParagraphId,
                        principalTable: "tbl_paragraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_usersComments_tbl_users_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_usersComments_ParagraphId",
                table: "tbl_usersComments",
                column: "ParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_usersComments_UserId",
                table: "tbl_usersComments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_usersComments");
        }
    }
}
