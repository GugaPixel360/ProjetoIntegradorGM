using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiEf.Migrations
{
    /// <inheritdoc />
    public partial class AjustarModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoUsuario",
                table: "Adotantes",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                table: "Adotantes");
        }
    }
}
