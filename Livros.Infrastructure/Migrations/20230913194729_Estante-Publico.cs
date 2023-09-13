using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livros.Infrastructure.Migrations
{
    public partial class EstantePublico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstanteObra_Estantes_ListasId",
                table: "EstanteObra");

            migrationBuilder.RenameColumn(
                name: "ListasId",
                table: "EstanteObra",
                newName: "EstantesId");

            migrationBuilder.AlterColumn<bool>(
                name: "Publico",
                table: "Estantes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150,
                oldDefaultValue: "Publico");

            migrationBuilder.AddForeignKey(
                name: "FK_EstanteObra_Estantes_EstantesId",
                table: "EstanteObra",
                column: "EstantesId",
                principalTable: "Estantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstanteObra_Estantes_EstantesId",
                table: "EstanteObra");

            migrationBuilder.RenameColumn(
                name: "EstantesId",
                table: "EstanteObra",
                newName: "ListasId");

            migrationBuilder.AlterColumn<string>(
                name: "Publico",
                table: "Estantes",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "Publico",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddForeignKey(
                name: "FK_EstanteObra_Estantes_ListasId",
                table: "EstanteObra",
                column: "ListasId",
                principalTable: "Estantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
