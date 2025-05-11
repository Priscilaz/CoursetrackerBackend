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

        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<Empleado>>> Listar()
        {
            return await _context.Empleados.ToListAsync();
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Empleado>> Crear(Empleado empleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Obtener), new { id = empleado.EmpleadoId }, empleado);
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Empleado>> Obtener(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            return empleado;
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Editar(int id, Empleado empleado)
        {
            if (id != empleado.EmpleadoId) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Empleados.Any(e => e.EmpleadoId == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("recomendar-cursos/{id}")]
        public async Task<ActionResult<IEnumerable<Curso>>> RecomendarCursos(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
                return NotFound("Empleado no encontrado.");

            var cursos = await _context.Cursos
                .Where(c => c.DuracionHoras <= empleado.HorasDisponibles)
                .ToListAsync();

            return cursos;
        }

    }

}
