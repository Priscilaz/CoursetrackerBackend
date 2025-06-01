using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseTracker.Models
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe contener exactamente 10 dígitos.")]
        public string Cedula { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [StringLength(150)]
        public string Email { get; set; }

        [Range(typeof(decimal), "0", "9999", ErrorMessage = "Las horas disponibles deben ser un número decimal ≥ 0.")]
        public decimal HorasDisponibles { get; set; }

        public ICollection<EmpleadoCurso> EmpleadoCursos { get; set; }
    }
}
