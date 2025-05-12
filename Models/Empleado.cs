using System.ComponentModel.DataAnnotations;

namespace CourseTracker.Models
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe tener exactamente 10 dígitos.")]
        public string Cedula { get; set; } = string.Empty;

        public float HorasDisponibles { get; set; }

        public ICollection<EmpleadoCurso> CursosAsignados { get; set; }

    }
}
