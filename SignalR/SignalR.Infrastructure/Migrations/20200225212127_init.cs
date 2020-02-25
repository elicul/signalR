using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalR.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Email = table.Column<string>(nullable: false),
                    TenantGuid = table.Column<Guid>(nullable: false),
                    TenantType = table.Column<string>(nullable: true),
                    ConnectionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConnectionId", "CreatedDateUtc", "Email", "LastModifiedDateUtc", "TenantGuid", "TenantType" },
                values: new object[] { new Guid("160071b0-a6f3-4832-a734-40d0e773f127"), "12345", new DateTime(2020, 2, 25, 21, 21, 26, 946, DateTimeKind.Utc).AddTicks(1669), "demo@demo.com", new DateTime(2020, 2, 25, 21, 21, 26, 946, DateTimeKind.Utc).AddTicks(1669), new Guid("3bf1ef56-84f5-434b-aaaf-e0a5040f87bd"), "Carrier" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
