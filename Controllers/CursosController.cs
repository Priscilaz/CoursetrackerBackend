using CourseTracker.Data;
using CourseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<Curso>>> Listar()
        {
            return await _context.Cursos.ToListAsync();
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Curso>> Crear(Curso curso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Obtener), new { id = curso.CursoId }, curso);
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Curso>> Obtener(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();
            return curso;
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Editar(int id, Curso curso)
        {
            if (id != curso.CursoId) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cursos.Any(e => e.CursoId == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
