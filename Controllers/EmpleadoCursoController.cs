using System;
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
    public class EmpleadoCursoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpleadoCursoController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/EmpleadoCurso/asignar
        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarCurso([FromBody] EmpleadoCursoCreateDTO dto)
        {
            // 1. Verificar que el empleado exista
            var empleado = await _context.Empleados.FindAsync(dto.EmpleadoId);
            if (empleado == null)
                return NotFound($"Empleado con ID {dto.EmpleadoId} no existe.");

            // 2. Verificar que el curso exista
            var curso = await _context.Cursos.FindAsync(dto.CursoId);
            if (curso == null)
                return NotFound($"Curso con ID {dto.CursoId} no existe.");

            // 3. Validar duplicado en la asignación
            bool yaAsignado = await _context.EmpleadoCursos
                .AnyAsync(ec => ec.EmpleadoId == dto.EmpleadoId && ec.CursoId == dto.CursoId);
            if (yaAsignado)
                return BadRequest("Este curso ya se encuentra asignado a este empleado.");

            // 4. Validar horas disponibles (ahora decimal)
            if (curso.DuracionHoras > empleado.HorasDisponibles)
                return BadRequest($"No hay horas suficientes. El curso dura {curso.DuracionHoras}h y el empleado tiene {empleado.HorasDisponibles}h.");

            // 5. Restar horas al empleado
            empleado.HorasDisponibles -= curso.DuracionHoras;

            // 6. Crear la asignación
            var asignacion = new EmpleadoCurso
            {
                EmpleadoId = dto.EmpleadoId,
                CursoId = dto.CursoId,
                FechaAsignacion = DateTime.UtcNow
            };
            _context.EmpleadoCursos.Add(asignacion);

            // 7. Guardar todo en una sola transacción
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error al guardar la asignación: {ex.Message}");
            }

            return Ok(new
            {
                Mensaje = "Curso asignado correctamente",
                HorasRestantes = empleado.HorasDisponibles
            });
        }

        // GET: api/EmpleadoCurso/empleado/5
        [HttpGet("empleado/{id}")]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursosAsignados(int id)
        {
            var cursos = await _context.EmpleadoCursos
                .Where(ec => ec.EmpleadoId == id)
                .Include(ec => ec.Curso)
                .Select(ec => ec.Curso)
                .ToListAsync();

            return cursos;
        }
    }
}
