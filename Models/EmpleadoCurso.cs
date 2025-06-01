using CourseTracker.Models;
using System.Text.Json.Serialization;

public class EmpleadoCurso
{
    

    public int EmpleadoId { get; set; }

    public Empleado Empleado { get; set; }

    public int CursoId { get; set; }

    
    public Curso Curso { get; set; }
    public DateTime FechaAsignacion { get; set; }
}
