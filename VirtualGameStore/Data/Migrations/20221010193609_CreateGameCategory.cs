using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualGameStore.Data.Migrations
{
    public partial class CreateGameCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameCategoryUser",
                columns: table => new
                {
                    FavoriteCategoriesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategoryUser", x => new { x.FavoriteCategoriesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GameCategoryUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameCategoryUser_GameCategories_FavoriteCategoriesId",
                        column: x => x.FavoriteCategoriesId,
                        principalTable: "GameCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCategoryUser_UsersId",
                table: "GameCategoryUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameCategoryUser");

            migrationBuilder.DropTable(
                name: "GameCategories");
        }
    }
}
