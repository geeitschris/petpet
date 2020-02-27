using Microsoft.EntityFrameworkCore.Migrations;

namespace petpet.Migrations
{
    public partial class thirddmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PetValue",
                table: "Pets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetValue",
                table: "Pets");
        }
    }
}
