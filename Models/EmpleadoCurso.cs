using CourseTracker.Models;
using System.Text.Json.Serialization;

public class EmpleadoCurso
{
    public int EmpleadoCursoId { get; set; }

    public int EmpleadoId { get; set; }

    [JsonIgnore]
    public Empleado? Empleado { get; set; }

    public int CursoId { get; set; }

    [JsonIgnore]
    public Curso? Curso { get; set; }
}
