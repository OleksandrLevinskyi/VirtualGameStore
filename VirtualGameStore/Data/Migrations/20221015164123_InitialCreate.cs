using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualGameStore.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailMarketingEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendeeLimit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IsDigital = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HolderFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HolderLastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentOptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryUser",
                columns: table => new
                {
                    FavoriteCategoriesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryUser", x => new { x.FavoriteCategoriesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_CategoryUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryUser_Categories_FavoriteCategoriesId",
                        column: x => x.FavoriteCategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    DateTimeRegistered = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Registrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryGame",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGame", x => new { x.CategoriesId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_CategoryGame_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryGame_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    PlatformsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => new { x.GamesId, x.PlatformsId });
                    table.ForeignKey(
                        name: "FK_GamePlatform_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatform_Platforms_PlatformsId",
                        column: x => x.PlatformsId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUser",
                columns: table => new
                {
                    FavoritePlatformsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUser", x => new { x.FavoritePlatformsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PlatformUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformUser_Platforms_FavoritePlatformsId",
                        column: x => x.FavoritePlatformsId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9a86cc44-771d-426d-b702-c4a4a93c348f", "00cce6e2-03f1-42cc-805f-91df7dae668e", "Member", "MEMBER" },
                    { "afe877ff-cf81-4bff-9d50-66238d3a1b9e", "6010635a-cb51-4a21-bb0a-529ece3facbc", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BillingAddressId", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "GenderId", "IsEmailMarketingEnabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShippingAddressId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5abf56ec-8224-42b1-965d-a11bd8d818c7", 0, null, null, "5e2e9d4a-d8e2-4200-8e13-ef777407f2ca", "employee@vgs.com", true, null, null, false, null, false, null, "EMPLOYEE@VGS.COM", "EMPLOYEE@VGS.COM", "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", null, false, "IOHH3QAG6CBJWWO4LDGAJTSSACV2KNDI", null, false, "employee@vgs.com" },
                    { "9a44a14a-47fb-4196-8a45-57fa557fb992", 0, null, null, "d91ba7e7-903d-453c-8caf-3ad1907f96c6", "member@vgs.com", true, null, null, false, null, true, null, "MEMBER@VGS.COM", "MEMBER@VGS.COM", "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", null, false, "HNGZXUROYPX527M6RHWO6OPYCETU2WVK", null, false, "member@vgs.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "RPG" },
                    { 2, "Racing" },
                    { 3, "Sports" },
                    { 4, "Simulation" },
                    { 5, "FPS" },
                    { 6, "Fighting" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Male" },
                    { 2, "Female" },
                    { 3, "Other" }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "PC" },
                    { 2, "Switch" },
                    { 3, "Xbox" },
                    { 4, "PlayStation" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "afe877ff-cf81-4bff-9d50-66238d3a1b9e", "5abf56ec-8224-42b1-965d-a11bd8d818c7" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9a86cc44-771d-426d-b702-c4a4a93c348f", "9a44a14a-47fb-4196-8a45-57fa557fb992" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingAddressId",
                table: "AspNetUsers",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GenderId",
                table: "AspNetUsers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShippingAddressId",
                table: "AspNetUsers",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryGame_GamesId",
                table: "CategoryGame",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUser_UsersId",
                table: "CategoryUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatorId",
                table: "Events",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_PlatformsId",
                table: "GamePlatform",
                column: "PlatformsId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOptions_UserId",
                table: "PaymentOptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUser_UsersId",
                table: "PlatformUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId",
                table: "Registrations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_UserId",
                table: "Registrations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_BillingAddressId",
                table: "AspNetUsers",
                column: "BillingAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_ShippingAddressId",
                table: "AspNetUsers",
                column: "ShippingAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Genders_GenderId",
                table: "AspNetUsers",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_BillingAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_ShippingAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Genders_GenderId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "CategoryGame");

            migrationBuilder.DropTable(
                name: "CategoryUser");

            migrationBuilder.DropTable(
                name: "GamePlatform");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "PaymentOptions");

            migrationBuilder.DropTable(
                name: "PlatformUser");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GenderId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShippingAddressId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "afe877ff-cf81-4bff-9d50-66238d3a1b9e", "5abf56ec-8224-42b1-965d-a11bd8d818c7" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9a86cc44-771d-426d-b702-c4a4a93c348f", "9a44a14a-47fb-4196-8a45-57fa557fb992" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a86cc44-771d-426d-b702-c4a4a93c348f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "afe877ff-cf81-4bff-9d50-66238d3a1b9e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5abf56ec-8224-42b1-965d-a11bd8d818c7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9a44a14a-47fb-4196-8a45-57fa557fb992");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsEmailMarketingEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "AspNetUsers");
        }
    }
}
