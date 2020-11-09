using Microsoft.EntityFrameworkCore.Migrations;

namespace IPAddressProccessor.API.Migrations
{
    public partial class processingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Processing",
                table: "BatchUpdateJobItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processing",
                table: "BatchUpdateJobItems");
        }
    }
}
