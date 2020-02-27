using Microsoft.EntityFrameworkCore.Migrations;

namespace petpet.Migrations
{
    public partial class whatevermigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "AllMail",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "AllMail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "AllMail");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "AllMail",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
