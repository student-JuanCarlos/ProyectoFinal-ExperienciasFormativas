using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Restaurante>Restaurantes { get; set; }
        public DbSet<TipoPlan>TipoPlan { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Platillo> Platillos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Descuento> Descuentos { get; set; }
        public DbSet<Mesa> Mesas { get; set;  }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<DetalleReserva> DetalleReserva { get; set; }
        public DbSet<ConfiguracionReserva> ConfiReserva { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<DetalleDescuento> DetalleDescuento { get; set; }
        public DbSet<Suscripcion> Suscripcion { get; set; }
        public DbSet<OrdenPago> OrdenPago { get; set; }
        public DbSet<TransaccionWebhook> Transaccion { get ; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Restaurante>(entity =>
            {
                entity.ToTable("Restaurante");
                entity.HasKey(r => r.IdRestaurante);
                entity.Property(r => r.FechaRegistro).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<TipoPlan>(entity =>
            {
                entity.ToTable("TipoPlan");
                entity.HasKey(t => t.IdTipoPlan);
                entity.Property(t => t.PrecioMensual).HasColumnType("decimal(10,2)");
            });

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

            modelBuilder.Entity<Mesa>(entity =>
            {
                entity.ToTable("Mesa", m => m.HasCheckConstraint("CK_Mesa_ValidationEstado", "Estado IN (1, 2, 3)"));
                entity.HasKey(m => m.IdMesa);
                entity.HasIndex(m => m.NumeroMesa).IsUnique();
                entity.Property(m => m.Estado).HasDefaultValue(1);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("Reserva", r =>
                {
                    r.HasCheckConstraint("CK_Reserva_ValidationTipoReserva", "TipoReserva IN ('Directa', 'Web')");
                    r.HasCheckConstraint("CK_Reserva_ValidationEstado", "Estado IN (1, 2, 3)"); //1 = Pendiente, 2 = Concluido, 3 = Cancelado
                    r.HasCheckConstraint("CK_Reserva_ValidationSesion", "IdCliente IS NOT NULL OR IdUsuario IS NOT NULL");
                });
                entity.HasKey(r => r.IdReserva);
                entity.Property(r => r.CostoTotal).HasColumnType("decimal(10, 2)");
                entity.Property(r => r.CostoTotal).HasDefaultValue(0);
                entity.Property(r => r.Estado).HasDefaultValue(1);
                entity.HasOne(r => r.cliente)
                       .WithMany()
                       .HasForeignKey(r => r.IdCliente);
                entity.HasOne(r => r.usuario)
                       .WithMany()
                       .HasForeignKey(r => r.IdUsuario);

            });

            modelBuilder.Entity<DetalleReserva>(entity =>
            {
                entity.ToTable("DetalleReserva");
                entity.HasKey(dr => dr.IdDetalleReserva);
                entity.HasOne(dr => dr.reserva)
                       .WithMany(dr => dr.DetalleMesa)
                       .HasForeignKey(dr => dr.IdReserva);
                entity.HasOne(dr => dr.mesa)
                       .WithMany()
                       .HasForeignKey(dr => dr.IdMesa);
            });

            modelBuilder.Entity<ConfiguracionReserva>(entity =>
            {
                entity.ToTable("ConfiguracionReserva", cr => cr.HasCheckConstraint("CK_Confi_ValidationID", "CHECK (IdConfiguration = 1)"));
                entity.HasKey(cr => cr.IdConfiguracion);
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.ToTable("Venta");
                entity.HasKey(v => v.IdVenta);
                entity.Property(v => v.FechaVenta).HasDefaultValueSql("GETDATE()");
                entity.HasOne(v => v.reserva)
                       .WithMany()
                       .HasForeignKey(v => v.IdReserva);
                entity.HasOne(v => v.usuario)
                       .WithMany()
                       .HasForeignKey(v => v.IdUsuario);
            });

            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.ToTable("DetalleVenta");
                entity.HasKey(dv => dv.IdDetalleVenta);
                entity.Property(dv => dv.PrecioUnitario).HasColumnType("decimal(5, 2)");
                entity.Property(dv => dv.SubTotal).HasComputedColumnSql("Cantidad * PrecioUnitario");
                entity.HasOne(dv => dv.venta)
                       .WithMany(dv => dv.detalles)
                       .HasForeignKey(dv => dv.IdVenta);
                entity.HasOne(dv => dv.platillo)
                       .WithMany()
                       .HasForeignKey(dv => dv.IdPlatillo);
            });

            modelBuilder.Entity<DetalleDescuento>(entity =>
            {
                entity.ToTable("DetalleDescuento");
                entity.HasKey(dd => dd.IdDetalleDescuento);
                entity.Property(dd => dd.DescuentoUnitario).HasColumnType("decimal(4, 2)");
                entity.HasOne(dd => dd.venta)
                       .WithMany(dd => dd.descuentos)
                       .HasForeignKey(dd => dd.IdVenta);
                entity.HasOne(dd => dd.descuento)
                       .WithMany()
                       .HasForeignKey(dd => dd.IdDescuento);
            });

            modelBuilder.Entity<Suscripcion>(entity =>
            {
                entity.ToTable("Suscripcion", s => s.HasCheckConstraint("CK_ValidationEstatus_Suscripcion", "Estado IN ('Activa', 'Vencida', 'Cancelada', 'Pendiente')"));
                entity.HasKey(s => s.IdSuscripcion);
                entity.Property(s => s.RenovacionAutomatica).HasDefaultValueSql("0");
                entity.HasOne(s => s.restaurante)
                       .WithMany()
                       .HasForeignKey(s => s.IdRestaurante);
                entity.HasOne(s => s.tipoPlan)
                       .WithMany()
                       .HasForeignKey(s => s.IdTipoPlan);
            });

            modelBuilder.Entity<OrdenPago>(entity =>
            {
                entity.ToTable("OrdenPago", o =>
                {
                    o.HasCheckConstraint("CK_ValidationMoneda", "Moneda IN ('PEN', 'USD')");
                    o.HasCheckConstraint("CK_ValidateStatus_OrdenPago", "Estado IN ('Pendiente', 'Procesando', 'Confirmado', 'Fallido', 'Expirado')");
                });

                entity.HasKey(o => o.IdOrdenPago);
                entity.Property(o => o.Monto).HasColumnType("decimal(10,2)");
                entity.Property(o => o.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.HasIndex(o => o.ReferenciaExterna).IsUnique();
                entity.HasOne(o => o.suscripcion)
                       .WithMany()
                       .HasForeignKey(o => o.IdSuscripcion);  
            });

            modelBuilder.Entity<TransaccionWebhook>(entity =>
            {
                entity.ToTable("TransaccionWebhook");
                entity.HasKey(t => t.IdTransaccionWebhook);
                entity.HasIndex(t => t.IdEventoExterno).IsUnique();
                entity.HasOne(t => t.ordenPago)
                       .WithMany()
                       .HasForeignKey(t => t.IdOrdenPago);
            });

        }

    }
}
