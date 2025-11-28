using Microsoft.EntityFrameworkCore;
using Seguros.API.Models;

namespace Seguros.API.Data
{
    public class SegurosDbContext : DbContext
    {
        public SegurosDbContext(DbContextOptions<SegurosDbContext> options) : base(options) { }

        public DbSet<TipoSeguro> TiposSeguro { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Cotizacion> Cotizaciones { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoSeguro>(b =>
            {
                b.HasKey(t => t.TipoSeguroId);
                b.Property(t => t.Nombre).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Cliente>(b =>
            {
                b.HasKey(c => c.ClienteId);
                b.HasIndex(c => c.Identidad).IsUnique();
                b.Property(c => c.Nombre).IsRequired().HasMaxLength(150);
                b.Property(c => c.CorreoElectronico).IsRequired().HasMaxLength(150);
                b.Property(c => c.TipoCliente).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<Cotizacion>(b =>
            {
                b.HasKey(c => c.CotizacionId);
                b.HasIndex(c => c.NumeroCotizacion).IsUnique();
                b.Property(c => c.NumeroCotizacion).IsRequired().HasMaxLength(30);
                b.Property(c => c.Moneda).IsRequired().HasMaxLength(10);
                b.Property(c => c.SumaAsegurada).HasColumnType("decimal(18,2)");
                b.Property(c => c.Tasa).HasColumnType("decimal(5,2)");
                b.HasOne(c => c.Cliente).WithMany(cl => cl.Cotizaciones).HasForeignKey(c => c.ClienteId);
                b.HasOne(c => c.TipoSeguro).WithMany(t => t.Cotizaciones).HasForeignKey(c => c.TipoSeguroId);
                b.HasIndex(c => c.FechaCotizacion);
                b.HasIndex(c => c.TipoSeguroId);
            });

            modelBuilder.Entity<TipoSeguro>().HasData(
                new TipoSeguro { TipoSeguroId = 1, Nombre = "Médico", Descripcion = "Seguro médico general" },
                new TipoSeguro { TipoSeguroId = 2, Nombre = "Automóvil", Descripcion = "Seguro para vehículos" },
                new TipoSeguro { TipoSeguroId = 3, Nombre = "Incendio", Descripcion = "Seguro contra incendios" },
                new TipoSeguro { TipoSeguroId = 4, Nombre = "Fianzas", Descripcion = "Seguro de fianzas" }
            );
        }
    }
}
