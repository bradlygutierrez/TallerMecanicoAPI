using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Database;

public class TallerDbContext(DbContextOptions<TallerDbContext> options, IUsuarioActual usuarioActual)
    : DbContext(options), IUnitOfWork
{
    public DbSet<EstadoExistencia> EstadosExistencia => Set<EstadoExistencia>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Marca> Marcas => Set<Marca>();
    public DbSet<Modelo> Modelos => Set<Modelo>();
    public DbSet<Servicio> Servicios => Set<Servicio>();
    public DbSet<EstadoOrden> EstadosOrden => Set<EstadoOrden>();
    public DbSet<ProblemaReportado> ProblemasReportados => Set<ProblemaReportado>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<OrdenTrabajo> OrdenesTrabajo => Set<OrdenTrabajo>();
    public DbSet<DetalleOrdenTrabajo> DetallesOrdenTrabajo => Set<DetalleOrdenTrabajo>();
    public DbSet<TotalOrdenTrabajo> TotalesOrden => Set<TotalOrdenTrabajo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TallerDbContext).Assembly);

        // La base usa NO ACTION en todas las FK: se replica aquí para no generar cascadas.
        foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AplicarAuditoria();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        AplicarAuditoria();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void AplicarAuditoria()
    {
        var ahora = DateTime.UtcNow;
        var usuarioId = usuarioActual.UsuarioId;

        foreach (var entry in ChangeTracker.Entries<EntidadAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion = ahora;
                entry.Entity.CreadoPorUsuarioId ??= usuarioId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaActualizacion = ahora;
                entry.Entity.ActualizadoPorUsuarioId = usuarioId;
                entry.Property(e => e.FechaCreacion).IsModified = false;
                entry.Property(e => e.CreadoPorUsuarioId).IsModified = false;
            }
        }
    }
}
