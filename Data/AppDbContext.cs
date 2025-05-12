using CourseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CourseTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<EmpleadoCurso> EmpleadoCursos { get; set; }

    }
}
