using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.Entidades;

public class MyAMISContext : DbContext
{
    public MyAMISContext(DbContextOptions<MyAMISContext> options)
        : base(options)
    {
    }

    // TABLAS BASE
    public DbSet<Activo> Activo { get; set; }
    public DbSet<TipoActivo> TipoActivo { get; set; }
    public DbSet<EstadoActivo> EstadoActivo { get; set; }

    // FALLAS
    public DbSet<Falla> Falla { get; set; }
    public DbSet<PrioridadFalla> PrioridadFalla { get; set; }
    public DbSet<EstadoFalla> EstadoFalla { get; set; }

    // MANTENIMIENTOS
    public DbSet<Mantenimiento> Mantenimiento { get; set; }
    public DbSet<TipoMantenimiento> TipoMantenimiento { get; set; }
    public DbSet<EstadoMantenimiento> EstadoMantenimiento { get; set; }

    // MOVIMIENTOS
    public DbSet<MovimientoActivo> MovimientoActivo { get; set; }

    // DOCUMENTOS
    public DbSet<TipoDocumento> TipoDocumento { get; set; }
    public DbSet<DocumentoActivo> DocumentoActivo { get; set; }
    public DbSet<Diagnostico> Diagnostico { get; set; }
    public DbSet<AuditoriaActivo> AuditoriaActivo { get; set; }
    public DbSet<AccionAuditoria> AccionAuditoria { get; set; }
    public DbSet<MetodoDepreciacion> MetodoDepreciacion { get; set; }
    public DbSet<DepreciacionActivo> DepreciacionActivo { get; set; }
    public DbSet<MantenimientoRepuesto> MantenimientoRepuesto { get; set; }
}