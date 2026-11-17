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
        // A coluna Senha já existe no banco.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        // Não remover a coluna existente.
        }
    }
}
