using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livros.Infrastructure.Migrations
{
    public partial class RelacionamentoObraEstante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstanteObra");

            migrationBuilder.AddColumn<Guid>(
                name: "ObraId",
                table: "Estantes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estantes_ObraId",
                table: "Estantes",
                column: "ObraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estantes_Obras_ObraId",
                table: "Estantes",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estantes_Obras_ObraId",
                table: "Estantes");

            migrationBuilder.DropIndex(
                name: "IX_Estantes_ObraId",
                table: "Estantes");

            migrationBuilder.DropColumn(
                name: "ObraId",
                table: "Estantes");

            migrationBuilder.CreateTable(
                name: "EstanteObra",
                columns: table => new
                {
                    EstantesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObrasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstanteObra", x => new { x.EstantesId, x.ObrasId });
                    table.ForeignKey(
                        name: "FK_EstanteObra_Estantes_EstantesId",
                        column: x => x.EstantesId,
                        principalTable: "Estantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstanteObra_Obras_ObrasId",
                        column: x => x.ObrasId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstanteObra_ObrasId",
                table: "EstanteObra",
                column: "ObrasId");
        }
    }
}