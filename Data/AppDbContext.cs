using CourseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<EmpleadoCurso> EmpleadoCursos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            

            // Índice único sobre Cedula
            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Cedula)
                .IsUnique();

            // Índice único sobre Nombre (evita duplicar nombres)
            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Nombre)
                .IsUnique();

            // Configurar precisión para HorasDisponibles (decimal)
            modelBuilder.Entity<Empleado>()
                .Property(e => e.HorasDisponibles)
                .HasPrecision(6, 2); // admite hasta 9999.99

            // Configurar precisión para DuracionHoras (decimal)
            modelBuilder.Entity<Curso>()
                .Property(c => c.DuracionHoras)
                .HasPrecision(6, 2); // admite hasta 9999.99

            // Configuración de la entidad puente EmpleadoCurso
            modelBuilder.Entity<EmpleadoCurso>()
                .HasKey(ec => new { ec.EmpleadoId, ec.CursoId });
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmpleadoCurso>()
                .HasOne(ec => ec.Empleado)
                .WithMany(e => e.EmpleadoCursos)
                .HasForeignKey(ec => ec.EmpleadoId);

            modelBuilder.Entity<EmpleadoCurso>()
                .HasOne(ec => ec.Curso)
                .WithMany(c => c.EmpleadoCursos)
                .HasForeignKey(ec => ec.CursoId);
        }
    }
}
