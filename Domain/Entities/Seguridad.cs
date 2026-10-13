namespace Domain.Entities;

public class EstadoExistencia : EntidadAuditable
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class Rol : EntidadAuditable
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}

public class Usuario : EntidadAuditable, IEntidadConEstado
{
    public int RolId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    /// <summary>Columna agregada con Scripts/02_AgregarPasswordHash.sql.</summary>
    public string? PasswordHash { get; set; }
    public int EstadoExistenciaId { get; set; }
    public Rol Rol { get; set; } = null!;
}
