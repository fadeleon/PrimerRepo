using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PruebaBD.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categorias { get; set; }

        public virtual DbSet<Producto> Productos { get; set; }

        public DbSet<ProductoCategorias> ProductoCategorias { get; set; }

        // DbSet para ApplicationUser
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DI-145466;Database=UnionDB;User Id=sa;Password=123456;Persist Security Info=False;Encrypt=False;MultipleActiveResultSets=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07DFCBAEAB");
                entity.Property(e => e.Nombre).HasMaxLength(100);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC074EF138B1");
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

                entity.HasMany(d => d.Categoria).WithMany(p => p.Productos)
                    .UsingEntity<ProductoCategorias>(
                        j => j
                            .HasOne(pc => pc.Categoria)
                            .WithMany(c => c.ProductoCategorias)
                            .HasForeignKey(pc => pc.CategoriaId)
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ProductoC__Categ__3C69FB99"),
                        j => j
                            .HasOne(pc => pc.Producto)
                            .WithMany(p => p.ProductoCategorias)
                            .HasForeignKey(pc => pc.ProductoId)
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ProductoC__Produ__3B75D760"),
                        j =>
                        {
                            j.HasKey("ProductoId", "CategoriaId").HasName("PK__Producto__EB0592BD93E2AC73");
                            j.ToTable("ProductoCategoria");
                        });
            });

            modelBuilder.Entity<ProductoCategorias>(entity =>
            {
                // Definición de la clave primaria compuesta
                entity.HasKey(pc => new { pc.ProductoId, pc.CategoriaId });

                // Relación entre Producto y ProductoCategorias
                entity.HasOne(pc => pc.Producto)
                    .WithMany(p => p.ProductoCategorias)
                    .HasForeignKey(pc => pc.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                // Relación entre Categoria y ProductoCategorias
                entity.HasOne(pc => pc.Categoria)
                    .WithMany(c => c.ProductoCategorias)
                    .HasForeignKey(pc => pc.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
