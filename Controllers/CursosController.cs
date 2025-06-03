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
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            return await _context.Cursos.ToListAsync();
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound("Curso no encontrado.");
            return curso;
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> CrearCurso(Curso curso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar nombre único de curso
            bool existe = await _context.Cursos
                .AnyAsync(c => c.Nombre.ToLower() == curso.Nombre.ToLower());
            if (existe)
                return BadRequest($"Ya existe un curso llamado '{curso.Nombre}'.");

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.CursoId }, curso);
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarCurso(int id, Curso curso)
        {
            if (id != curso.CursoId)
                return BadRequest("El ID no coincide.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cursoExistente = await _context.Cursos.FindAsync(id);
            if (cursoExistente == null)
                return NotFound("Curso no existe.");

            // Si cambió el nombre, validar duplicados
            if (!cursoExistente.Nombre.Equals(curso.Nombre, System.StringComparison.OrdinalIgnoreCase))
            {
                bool existeNombre = await _context.Cursos
                    .AnyAsync(c => c.Nombre.ToLower() == curso.Nombre.ToLower() && c.CursoId != id);
                if (existeNombre)
                    return BadRequest($"Otro curso ya tiene el nombre '{curso.Nombre}'.");
            }

            // Actualizar campos
            cursoExistente.Nombre = curso.Nombre;
            cursoExistente.Descripcion = curso.Descripcion;
            cursoExistente.DuracionHoras = curso.DuracionHoras;

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

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound("Curso no existe.");

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        

    }
}
