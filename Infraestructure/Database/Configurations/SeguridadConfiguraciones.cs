using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Database.Configurations;

public class EstadoExistenciaConfig : IEntityTypeConfiguration<EstadoExistencia>
{
    public void Configure(EntityTypeBuilder<EstadoExistencia> b)
    {
        b.Tabla("EstadoExistencia", "catalogos", "EstadoExistenciaId");
        b.Auditoria(creadorRequerido: false);
        b.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
        b.Property(e => e.Descripcion).HasMaxLength(250);
        b.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("UQ_EstadoExistencia_Nombre");
    }
}

public class RolConfig : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> b)
    {
        b.Tabla("Rol", "seguridad", "RolId");
        b.Auditoria(creadorRequerido: false);
        b.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
        b.Property(e => e.Descripcion).HasMaxLength(250);
        b.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("UQ_Rol_Nombre");
    }
}

public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.Tabla("Usuario", "seguridad", "UsuarioId");
        b.Auditoria(creadorRequerido: false);
        b.Property(e => e.NombreUsuario).HasMaxLength(80).IsRequired();
        b.Property(e => e.Telefono).HasMaxLength(25);
        b.Property(e => e.PasswordHash).HasMaxLength(500);
        b.HasIndex(e => e.NombreUsuario).IsUnique().HasDatabaseName("UQ_Usuario_NombreUsuario");
        b.HasOne(e => e.Rol).WithMany(r => r.Usuarios).HasForeignKey(e => e.RolId);
        b.HasOne<EstadoExistencia>().WithMany().HasForeignKey(e => e.EstadoExistenciaId);
        b.HasQueryFilter(e => e.EstadoExistenciaId == EstadoExistenciaIds.Activo);
    }
}
