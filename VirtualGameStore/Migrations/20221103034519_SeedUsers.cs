using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualGameStore.Migrations
{
    public partial class SeedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BillingAddressId", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "GenderId", "IsEmailMarketingEnabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShippingAddressId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "76742c46-0008-4749-af77-5d129b6d88b1", 0, null, null, "0c6c0c5a-52ea-4b4c-89cc-8130611f1e54", "bar@vgs.com", true, null, null, false, null, true, null, "BAR@VGS.COM", "BAR", "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", null, false, "7XDDKAH2YGWTBDC7UVPT76DUXTLQES3E", null, false, "bar" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BillingAddressId", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "GenderId", "IsEmailMarketingEnabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShippingAddressId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d5dafa9f-92a4-43dc-9652-02cf3860d621", 0, null, null, "0f4fa02d-33c6-48e6-b573-7218fa00c9a2", "foo@vgs.com", true, null, null, false, null, true, null, "FOO@VGS.COM", "FOO", "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", null, false, "WQ7TGOMDYEUVSMNVX2G35VKZ4MPGODG4", null, false, "foo" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9a86cc44-771d-426d-b702-c4a4a93c348f", "76742c46-0008-4749-af77-5d129b6d88b1" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9a86cc44-771d-426d-b702-c4a4a93c348f", "d5dafa9f-92a4-43dc-9652-02cf3860d621" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9a86cc44-771d-426d-b702-c4a4a93c348f", "76742c46-0008-4749-af77-5d129b6d88b1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9a86cc44-771d-426d-b702-c4a4a93c348f", "d5dafa9f-92a4-43dc-9652-02cf3860d621" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "76742c46-0008-4749-af77-5d129b6d88b1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5dafa9f-92a4-43dc-9652-02cf3860d621");
        }
    }
}
