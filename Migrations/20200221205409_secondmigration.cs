using Microsoft.EntityFrameworkCore.Migrations;

namespace petpet.Migrations
{
    public partial class secondmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PetBio",
                table: "Pets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PetBio",
                table: "Pets",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
