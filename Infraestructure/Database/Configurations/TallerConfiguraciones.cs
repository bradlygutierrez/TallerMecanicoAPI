using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Database.Configurations;

public class OrdenTrabajoConfig : IEntityTypeConfiguration<OrdenTrabajo>
{
    public void Configure(EntityTypeBuilder<OrdenTrabajo> b)
    {
        b.Tabla("OrdenTrabajo", "taller", "OrdenTrabajoId");
        b.Auditoria();
        b.Property(e => e.FechaIngreso).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSUTCDATETIME()");
        b.HasIndex(e => new { e.VehiculoId, e.FechaIngreso }).IsDescending(false, true)
            .HasDatabaseName("IX_OrdenTrabajo_Vehiculo_FechaIngreso");
        b.HasOne(e => e.Vehiculo).WithMany().HasForeignKey(e => e.VehiculoId);
        b.HasOne(e => e.ProblemaReportado).WithMany().HasForeignKey(e => e.ProblemaReportadoId);
        b.HasOne(e => e.EstadoOrden).WithMany().HasForeignKey(e => e.EstadoOrdenId);
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}

public class DetalleOrdenTrabajoConfig : IEntityTypeConfiguration<DetalleOrdenTrabajo>
{
    public void Configure(EntityTypeBuilder<DetalleOrdenTrabajo> b)
    {
        b.Tabla("DetalleOrdenTrabajo", "taller", "DetalleOrdenTrabajoId");
        b.Auditoria();
        b.Property(e => e.Cantidad).HasPrecision(12, 2);
        b.Property(e => e.PrecioUnitarioAplicado).HasPrecision(18, 2);
        // Columna calculada y persistida: EF solo la lee, nunca la escribe.
        b.Property(e => e.Subtotal)
            .HasColumnType("decimal(30,4)")
            .HasComputedColumnSql("CONVERT(DECIMAL(30,4), [Cantidad] * [PrecioUnitarioAplicado])", stored: true);
        b.HasIndex(e => new { e.OrdenTrabajoId, e.ServicioId }).IsUnique()
            .HasDatabaseName("UQ_DetalleOrdenTrabajo_Orden_Servicio");
        b.HasOne(e => e.OrdenTrabajo).WithMany(o => o.Detalles).HasForeignKey(e => e.OrdenTrabajoId);
        b.HasOne(e => e.Servicio).WithMany().HasForeignKey(e => e.ServicioId);
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}

public class TotalOrdenTrabajoConfig : IEntityTypeConfiguration<TotalOrdenTrabajo>
{
    public void Configure(EntityTypeBuilder<TotalOrdenTrabajo> b)
    {
        b.HasNoKey().ToView("vw_TotalOrdenTrabajo", "taller");
        b.Property(e => e.Total).HasColumnType("decimal(38,4)");
    }
}
