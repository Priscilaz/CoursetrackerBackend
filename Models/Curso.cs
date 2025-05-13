namespace CourseTracker.Models
{
    public class Curso
    {
        public int CursoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public float DuracionHoras { get; set; }
        public ICollection<EmpleadoCurso> EmpleadosAsignados { get; set; } = new List<EmpleadoCurso>();

    }
}
