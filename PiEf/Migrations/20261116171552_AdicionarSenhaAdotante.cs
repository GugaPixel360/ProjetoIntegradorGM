using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiEf.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarSenhaAdotante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Adotantes",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Adotantes");
        }
    }
}
