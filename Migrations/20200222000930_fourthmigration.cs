using Microsoft.EntityFrameworkCore.Migrations;

namespace petpet.Migrations
{
    public partial class fourthmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PetBio",
                table: "Pets",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "AllMail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "AllMail");

            migrationBuilder.AlterColumn<string>(
                name: "PetBio",
                table: "Pets",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250);
        }
    }
}
