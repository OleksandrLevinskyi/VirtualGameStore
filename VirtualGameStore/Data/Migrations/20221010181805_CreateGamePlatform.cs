using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualGameStore.Data.Migrations
{
    public partial class CreateGamePlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamePlatforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatformUser",
                columns: table => new
                {
                    FavoritePlatformsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatformUser", x => new { x.FavoritePlatformsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GamePlatformUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatformUser_GamePlatforms_FavoritePlatformsId",
                        column: x => x.FavoritePlatformsId,
                        principalTable: "GamePlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatformUser_UsersId",
                table: "GamePlatformUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePlatformUser");

            migrationBuilder.DropTable(
                name: "GamePlatforms");
        }
    }
}
