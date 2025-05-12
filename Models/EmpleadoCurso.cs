namespace CourseTracker.Models
{
    public class EmpleadoCurso
    {
        public int EmpleadoCursoId { get; set; }

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}
