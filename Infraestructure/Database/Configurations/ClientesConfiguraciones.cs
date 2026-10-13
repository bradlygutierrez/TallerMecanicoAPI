using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Database.Configurations;

public class ClienteConfig : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> b)
    {
        b.Tabla("Cliente", "clientes", "ClienteId");
        b.Auditoria();
        b.Property(e => e.NombreCompleto).HasMaxLength(150).IsRequired();
        b.Property(e => e.Telefono).HasMaxLength(25);
        b.Property(e => e.CorreoElectronico).HasMaxLength(254);
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}

public class VehiculoConfig : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> b)
    {
        b.Tabla("Vehiculo", "clientes", "VehiculoId");
        b.Auditoria();
        b.Property(e => e.Placa).HasMaxLength(20).IsRequired();
        b.HasIndex(e => e.Placa).IsUnique().HasDatabaseName("UQ_Vehiculo_Placa");
        b.HasOne(e => e.Cliente).WithMany(c => c.Vehiculos).HasForeignKey(e => e.ClienteId);
        b.HasOne(e => e.Marca).WithMany().HasForeignKey(e => e.MarcaId);
        // FK compuesta (MarcaId, ModeloId): garantiza que el modelo pertenezca a la marca.
        b.HasOne(e => e.Modelo).WithMany(m => m.Vehiculos)
            .HasForeignKey(e => new { e.MarcaId, e.ModeloId })
            .HasPrincipalKey(m => new { m.MarcaId, m.Id });
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}
