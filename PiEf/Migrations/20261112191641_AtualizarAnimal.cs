using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiEf.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponivel",
                table: "Animais",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Especie",
                table: "Animais",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Idade",
                table: "Animais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Animais",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Raca",
                table: "Animais",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Animais",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponivel",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Especie",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Idade",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Raca",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Animais");
        }
    }
}
