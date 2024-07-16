using Microsoft.EntityFrameworkCore;
using TestAoniken.Models;

#nullable disable

namespace TestAoniken.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Publicacion> Publicaciones { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Publicacion>(entity =>
            {
                entity.HasIndex(e => e.AutorId, "IX_Publicaciones_AutorId");

                entity.Property(e => e.Contenido).IsRequired();

                entity.Property(e => e.Titulo).IsRequired();

                entity.HasOne(d => d.Autor)
                    .WithMany(p => p.Publicaciones)
                    .HasForeignKey(d => d.AutorId);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Rol).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
