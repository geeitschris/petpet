using Microsoft.EntityFrameworkCore.Migrations;

namespace petpet.Migrations
{
    public partial class anothermigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdult",
                table: "Pets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdult",
                table: "Pets");
        }
    }
}
