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

        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarCurso([FromBody] EmpleadoCurso ec)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existe = await _context.EmpleadoCursos
                .AnyAsync(x => x.EmpleadoId == ec.EmpleadoId && x.CursoId == ec.CursoId);

            if (existe)
                return BadRequest("Este curso ya está asignado al empleado.");

            _context.EmpleadoCursos.Add(ec);
            await _context.SaveChangesAsync();
            return Ok(ec);
        }

        [HttpGet("empleado/{id}")]
        public async Task<ActionResult<IEnumerable<Curso>>> CursosAsignados(int id)
        {
            var cursos = await _context.EmpleadoCursos
                .Where(x => x.EmpleadoId == id)
                .Include(x => x.Curso)
                .Select(x => x.Curso)
                .ToListAsync();

            return cursos;
        }
    }
}
