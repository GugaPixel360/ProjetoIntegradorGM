using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiEf.Migrations
{
    public partial class AdicionarSenhaAdotante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Adotantes",
                type: "longtext",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Adotantes");
        }
    }
}