using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddEmpleadoCurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpleadoCursos",
                columns: table => new
                {
                    EmpleadoCursoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoCursos", x => x.EmpleadoCursoId);
                    table.ForeignKey(
                        name: "FK_EmpleadoCursos_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "CursoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadoCursos_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "EmpleadoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoCursos_CursoId",
                table: "EmpleadoCursos",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoCursos_EmpleadoId",
                table: "EmpleadoCursos",
                column: "EmpleadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpleadoCursos");
        }
    }
}
