using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livros.Infrastructure.Migrations
{
    public partial class ListaAlteradoEstante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListaObra");

            migrationBuilder.DropTable(
                name: "Listas");

            migrationBuilder.CreateTable(
                name: "Estantes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Publico = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, defaultValue: "Publico"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estantes_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstanteObra",
                columns: table => new
                {
                    ListasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObrasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstanteObra", x => new { x.ListasId, x.ObrasId });
                    table.ForeignKey(
                        name: "FK_EstanteObra_Estantes_ListasId",
                        column: x => x.ListasId,
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

            migrationBuilder.CreateIndex(
                name: "IX_Estantes_UsuarioId",
                table: "Estantes",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstanteObra");

            migrationBuilder.DropTable(
                name: "Estantes");

            migrationBuilder.CreateTable(
                name: "Listas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Publico = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, defaultValue: "Publico"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listas_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ListaObra",
                columns: table => new
                {
                    ListasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObrasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaObra", x => new { x.ListasId, x.ObrasId });
                    table.ForeignKey(
                        name: "FK_ListaObra_Listas_ListasId",
                        column: x => x.ListasId,
                        principalTable: "Listas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaObra_Obras_ObrasId",
                        column: x => x.ObrasId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListaObra_ObrasId",
                table: "ListaObra",
                column: "ObrasId");

            migrationBuilder.CreateIndex(
                name: "IX_Listas_UsuarioId",
                table: "Listas",
                column: "UsuarioId");
        }
    }
}