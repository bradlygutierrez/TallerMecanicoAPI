namespace Domain.Entities;

/// <summary>Base de todas las entidades. Id se mapea a la columna "{Entidad}Id" de cada tabla.</summary>
public abstract class Entidad
{
    public int Id { get; set; }
}

/// <summary>Columnas de auditoría presentes en casi todas las tablas.</summary>
public abstract class EntidadAuditable : Entidad
{
    public DateTime FechaCreacion { get; set; }
    // Nullable en C# porque EstadoExistencia, Rol y Usuario lo admiten en el arranque inicial.
    // En el resto de tablas la columna es NOT NULL (se configura en Infraestructure).
    public int? CreadoPorUsuarioId { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public int? ActualizadoPorUsuarioId { get; set; }
}

/// <summary>Entidades con borrado lógico mediante EstadoExistenciaId.</summary>
public interface IEntidadConEstado
{
    int EstadoExistenciaId { get; set; }
}
