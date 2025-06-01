using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseTracker.Models
{
    public class Curso
    {
        public int CursoId { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Range(typeof(decimal), "0.1", "9999", ErrorMessage = "La duración debe ser un número decimal positivo.")]
        public decimal DuracionHoras { get; set; }

        public ICollection<EmpleadoCurso> EmpleadoCursos { get; set; }
    }
}
