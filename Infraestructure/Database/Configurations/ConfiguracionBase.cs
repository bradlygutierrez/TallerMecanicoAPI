using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Database.Configurations;

internal static class ConfiguracionBase
{
    /// <summary>Tabla, esquema, clave primaria y nombre de la columna del Id.</summary>
    public static void Tabla<T>(this EntityTypeBuilder<T> b, string tabla, string esquema, string columnaId)
        where T : Entidad
    {
        b.ToTable(tabla, esquema);
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName(columnaId);
    }

    /// <summary>Columnas de auditoría. creadorRequerido = false solo para EstadoExistencia, Rol y Usuario.</summary>
    public static void Auditoria<T>(this EntityTypeBuilder<T> b, bool creadorRequerido = true)
        where T : EntidadAuditable
    {
        b.Property(e => e.FechaCreacion).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSUTCDATETIME()");
        b.Property(e => e.FechaActualizacion).HasColumnType("datetime2(0)");
        b.Property(e => e.CreadoPorUsuarioId).IsRequired(creadorRequerido);
    }
}
