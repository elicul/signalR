using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalR.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
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
                columns: new[] { "Id", "ConnectionId", "CreatedBy", "CreatedDateUtc", "Email", "LastModifiedBy", "LastModifiedDateUtc", "TenantGuid", "TenantType" },
                values: new object[] { new Guid("06bfc2e0-a474-471f-8a0a-28cc2e3e5bb1"), "12345", "System", new DateTime(2020, 2, 24, 22, 5, 13, 50, DateTimeKind.Utc).AddTicks(2085), "demo@demo.com", "System", new DateTime(2020, 2, 24, 22, 5, 13, 50, DateTimeKind.Utc).AddTicks(2085), new Guid("a8aeb35c-02cf-4626-9062-934eb2ac3580"), "Carrier" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
