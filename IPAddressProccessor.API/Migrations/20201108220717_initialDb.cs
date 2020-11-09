using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IPAddressProccessor.API.Migrations
{
    public partial class initialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchUpdateJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Complete = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchUpdateJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IPAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Continent = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BatchUpdateJobItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IpToUpdate = table.Column<string>(nullable: true),
                    UpdateComplete = table.Column<bool>(nullable: false),
                    UpdateJobId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchUpdateJobItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchUpdateJobItems_BatchUpdateJobs_UpdateJobId",
                        column: x => x.UpdateJobId,
                        principalTable: "BatchUpdateJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchUpdateJobItems_UpdateJobId",
                table: "BatchUpdateJobItems",
                column: "UpdateJobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchUpdateJobItems");

            migrationBuilder.DropTable(
                name: "IPAddresses");

            migrationBuilder.DropTable(
                name: "BatchUpdateJobs");
        }
    }
}
