using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseTracker.Data;
using CourseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpleadosController(AppDbContext context)
        {
            _context = context;
        }
        ///CAMBIOS

        [HttpGet("cursoMasAsignado")]
        public async Task<IActionResult> GetCursoMasAsignado()
        {
            
            var grupo = await _context.EmpleadoCursos
                .GroupBy(ec => ec.CursoId)
                .Select(g => new
                {
                    CursoId = g.Key,
                    Asignaciones = g.Count()
                })
                .OrderByDescending(x => x.Asignaciones)
                .FirstOrDefaultAsync();

            if (grupo == null)
            {
                return NotFound("No hay cursos asignados a ningún empleado.");
            }

          
            var curso = await _context.Cursos.FindAsync(grupo.CursoId);
            if (curso == null)
            {
                return NotFound($"No se encontró el curso con ID {grupo.CursoId}.");
            }

            
            return Ok(new
            {
                Curso = new
                {
                    curso.CursoId,
                    curso.Nombre,
                    curso.DuracionHoras
                },
                CantidadAsignaciones = grupo.Asignaciones
            });
        }



        [HttpGet("personaMasLibre")]
        public async Task<IActionResult> GetPersonaMasLibre()
        {
           

            var consulta = await _context.Empleados
                .Select(e => new
                {
                    Empleado = e,
                    HorasUsadas = e.EmpleadoCursos.Sum(ec => ec.Curso.DuracionHoras)
                })
                .Select(x => new
                {
                    x.Empleado.EmpleadoId,
                    x.Empleado.Nombre,
                    x.Empleado.HorasDisponibles,
                    HorasUsadas = x.HorasUsadas,
                    HorasLibres = x.Empleado.HorasDisponibles - x.HorasUsadas
                })
                .OrderByDescending(x => x.HorasLibres)
                .FirstOrDefaultAsync();

            if (consulta == null)
            {
                return NotFound("No se encontró ningún empleado.");
            }

            return Ok(new
            {
                Empleado = new
                {
                    consulta.EmpleadoId,
                    consulta.Nombre,
                    consulta.HorasDisponibles
                },
                HorasUsadas = consulta.HorasUsadas,
                HorasLibres = consulta.HorasLibres
            });
        }

        [HttpGet("horasPorCurso")]
        public async Task<IActionResult> GetHorasPorCurso()
        {
           
            var resultado = await _context.Cursos
                .Select(c => new
                {
                    CursoId = c.CursoId,
                    c.Nombre,
                    c.DuracionHoras,
                    
                    CantidadAsignaciones = c.EmpleadoCursos.Count(),
                    
                    TotalHorasAsignadas = c.EmpleadoCursos.Count() * c.DuracionHoras
                })
                .ToListAsync();

            
            if (resultado == null || !resultado.Any())
            {
                return NotFound("No se encontró ningún curso.");
            }

            return Ok(resultado);
        }

        ///CAMBIOS

        // GET: api/Empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            return await _context.Empleados.ToListAsync();
        }

        // GET: api/Empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return NotFound("Empleado no encontrado.");
            return empleado;
        }

        // POST: api/Empleados
        [HttpPost]
        public async Task<ActionResult<Empleado>> CrearEmpleado(Empleado empleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar cédula única
            bool existeCedula = await _context.Empleados
                .AnyAsync(e => e.Cedula == empleado.Cedula);
            if (existeCedula)
                return BadRequest($"Ya existe un empleado con cédula '{empleado.Cedula}'.");

            // Validar nombre único
            bool existeNombre = await _context.Empleados
                .AnyAsync(e => e.Nombre.ToLower() == empleado.Nombre.ToLower());
            if (existeNombre)
                return BadRequest($"Ya existe un empleado con nombre '{empleado.Nombre}'.");

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.EmpleadoId }, empleado);
        }

        // PUT: api/Empleados/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarEmpleado(int id, Empleado empleado)
        {
            if (id != empleado.EmpleadoId)
                return BadRequest("El ID de la URL no coincide con el del payload.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var empleadoExistente = await _context.Empleados.FindAsync(id);
            if (empleadoExistente == null)
                return NotFound("Empleado no encontrado.");

            // Si cambió la cédula, validar duplicados
            if (empleadoExistente.Cedula != empleado.Cedula)
            {
                bool existeCedula = await _context.Empleados
                    .AnyAsync(e => e.Cedula == empleado.Cedula && e.EmpleadoId != id);
                if (existeCedula)
                    return BadRequest($"Ya existe otro empleado con cédula '{empleado.Cedula}'.");
            }

            // Si cambió el nombre, validar duplicados
            if (!empleadoExistente.Nombre.Equals(empleado.Nombre, System.StringComparison.OrdinalIgnoreCase))
            {
                bool existeNombre = await _context.Empleados
                    .AnyAsync(e => e.Nombre.ToLower() == empleado.Nombre.ToLower() && e.EmpleadoId != id);
                if (existeNombre)
                    return BadRequest($"Ya existe otro empleado con nombre '{empleado.Nombre}'.");
            }

            // Actualizar campos permitidos
            empleadoExistente.Nombre = empleado.Nombre;
            empleadoExistente.Cedula = empleado.Cedula;
            empleadoExistente.Email = empleado.Email;
            empleadoExistente.HorasDisponibles = empleado.HorasDisponibles;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error al actualizar: {ex.Message}");
            }

            return NoContent();
        }

        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return NotFound("Empleado no existe.");

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Empleados/5/cursos-recomendados
        [HttpGet("{id}/cursos-recomendados")]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursosRecomendados(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return NotFound("Empleado no encontrado.");

            // Solo devolver los cursos cuya DuracionHoras ≤ HorasDisponibles del empleado
            var cursosValidos = await _context.Cursos
                .Where(c => c.DuracionHoras <= empleado.HorasDisponibles)
                .ToListAsync();

            return cursosValidos;
        }


        
        

    }
}
