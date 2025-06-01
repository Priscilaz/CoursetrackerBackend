using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTracker.Migrations
{
    /// <inheritdoc />
    public partial class EmpleadoCursoValidacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmpleadoCursos",
                table: "EmpleadoCursos");

            migrationBuilder.DropIndex(
                name: "IX_EmpleadoCursos_EmpleadoId",
                table: "EmpleadoCursos");

            migrationBuilder.DropColumn(
                name: "EmpleadoCursoId",
                table: "EmpleadoCursos");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Empleados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HorasDisponibles",
                table: "Empleados",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Empleados",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAsignacion",
                table: "EmpleadoCursos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Cursos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DuracionHoras",
                table: "Cursos",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmpleadoCursos",
                table: "EmpleadoCursos",
                columns: new[] { "EmpleadoId", "CursoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Cedula",
                table: "Empleados",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Nombre",
                table: "Empleados",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Empleados_Cedula",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_Nombre",
                table: "Empleados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmpleadoCursos",
                table: "EmpleadoCursos");

            migrationBuilder.DropColumn(
                name: "FechaAsignacion",
                table: "EmpleadoCursos");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<float>(
                name: "HorasDisponibles",
                table: "Empleados",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoCursoId",
                table: "EmpleadoCursos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Cursos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<float>(
                name: "DuracionHoras",
                table: "Cursos",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmpleadoCursos",
                table: "EmpleadoCursos",
                column: "EmpleadoCursoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoCursos_EmpleadoId",
                table: "EmpleadoCursos",
                column: "EmpleadoId");
        }
    }
}
