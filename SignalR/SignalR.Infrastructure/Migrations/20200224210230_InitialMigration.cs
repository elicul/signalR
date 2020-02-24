using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalR.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
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
                    Email = table.Column<string>(nullable: true),
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
                values: new object[] { new Guid("ac6d6c33-6bbf-4446-888d-3292e5c181ea"), null, "System", new DateTime(2020, 2, 24, 21, 2, 29, 963, DateTimeKind.Utc).AddTicks(188), "demo@demo.com", "System", new DateTime(2020, 2, 24, 21, 2, 29, 963, DateTimeKind.Utc).AddTicks(188), new Guid("00000000-0000-0000-0000-000000000000"), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
