using Microsoft.EntityFrameworkCore.Migrations;

namespace petpet.Migrations
{
    public partial class somethingsomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PetComments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PetComments",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
