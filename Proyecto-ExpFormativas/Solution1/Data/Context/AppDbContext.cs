using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Platillo> Platillos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Descuento> Descuentos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Platillo>(entity =>
            {
                entity.ToTable("Platillo");
                entity.HasKey(p => p.IdPlatillo);
                entity.Property(p => p.Precio).HasColumnType("decimal(5,2)");

                entity.HasOne(p => p.categoria)
                       .WithMany()
                       .HasForeignKey(p => p.IdCategoria);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categoria");
                entity.HasKey(c => c.IdCategoria);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");
                entity.HasKey(c => c.IdCliente);
                entity.HasIndex(c => c.Documento).IsUnique();
                entity.HasIndex(c => c.Email).IsUnique();
            });

            modelBuilder.Entity<Descuento>(entity =>
            {
                entity.ToTable("Descuento", d => d.HasCheckConstraint("CK_Descuento_Tipo", "TipoDescuento IN ('Sin Fecha', 'Con Fecha')"));
                entity.HasKey(d => d.IdDescuento);
                entity.Property(d => d.PorcentajeDescuento).HasColumnType("decimal(4,2)");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");
                entity.HasKey(u => u.IdUsuario);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(u => u.cargo)
                    .WithMany()
                    .HasForeignKey(u => u.IdCargo);

                entity.HasOne(u => u.rol)
                       .WithMany()
                       .HasForeignKey(u => u.IdRol);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");
                entity.HasKey(r => r.IdRol);
            });

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.ToTable("Cargo");
                entity.HasKey(c => c.IdCargo);
            });
        }

    }
}
