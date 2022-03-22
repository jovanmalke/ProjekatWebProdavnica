using Microsoft.EntityFrameworkCore.Migrations;

namespace PrWeb.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cena",
                table: "ProizvodNarudzbina",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cena",
                table: "ProizvodNarudzbina");
        }
    }
}
