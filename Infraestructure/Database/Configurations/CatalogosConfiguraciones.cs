using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Database.Configurations;

public class MarcaConfig : IEntityTypeConfiguration<Marca>
{
    public void Configure(EntityTypeBuilder<Marca> b)
    {
        b.Tabla("Marca", "catalogos", "MarcaId");
        b.Auditoria();
        b.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        b.Property(e => e.PaisOrigen).HasMaxLength(100);
        b.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("UQ_Marca_Nombre");
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}

public class ModeloConfig : IEntityTypeConfiguration<Modelo>
{
    public void Configure(EntityTypeBuilder<Modelo> b)
    {
        b.Tabla("Modelo", "catalogos", "ModeloId");
        b.Auditoria();
        b.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        b.Property(e => e.Descripcion).HasMaxLength(250);
        // UQ_Modelo_MarcaId_ModeloId: clave alterna usada por la FK compuesta de Vehiculo.
        b.HasAlternateKey(e => new { e.MarcaId, e.Id }).HasName("UQ_Modelo_MarcaId_ModeloId");
        b.HasIndex(e => new { e.MarcaId, e.Nombre }).IsUnique().HasDatabaseName("UQ_Modelo_Marca_Nombre");
        b.HasOne(e => e.Marca).WithMany(m => m.Modelos).HasForeignKey(e => e.MarcaId);
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}

public class ServicioConfig : IEntityTypeConfiguration<Servicio>
{
    public void Configure(EntityTypeBuilder<Servicio> b)
    {
        b.Tabla("Servicio", "catalogos", "ServicioId");
        b.Auditoria();
        b.Property(e => e.Nombre).HasMaxLength(120).IsRequired();
        b.Property(e => e.Descripcion).HasMaxLength(500);
        b.Property(e => e.PrecioBase).HasPrecision(18, 2);
        b.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("UQ_Servicio_Nombre");
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}

public class EstadoOrdenConfig : IEntityTypeConfiguration<EstadoOrden>
{
    public void Configure(EntityTypeBuilder<EstadoOrden> b)
    {
        b.Tabla("EstadoOrden", "catalogos", "EstadoOrdenId");
        b.Auditoria();
        b.Property(e => e.Nombre).HasMaxLength(60).IsRequired();
        b.Property(e => e.Descripcion).HasMaxLength(250);
        b.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("UQ_EstadoOrden_Nombre");
    }
}

public class ProblemaReportadoConfig : IEntityTypeConfiguration<ProblemaReportado>
{
    public void Configure(EntityTypeBuilder<ProblemaReportado> b)
    {
        b.Tabla("ProblemaReportado", "catalogos", "ProblemaReportadoId");
        b.Auditoria();
        b.Property(e => e.Nombre).HasMaxLength(120).IsRequired();
        b.Property(e => e.Descripcion).HasMaxLength(500);
        b.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("UQ_ProblemaReportado_Nombre");
    }
}
