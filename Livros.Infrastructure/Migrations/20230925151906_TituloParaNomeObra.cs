using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livros.Infrastructure.Migrations
{
    public partial class TituloParaNomeObra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Obras",
                newName: "Nome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Obras",
                newName: "Titulo");
        }
    }
}