namespace Domain.Interfaces;

/// <summary>Usuario que ejecuta la operación; se usa para llenar las columnas de auditoría.</summary>
public interface IUsuarioActual
{
    /// <summary>Id del usuario autenticado, o el usuario 'sistema' si no hay sesión.</summary>
    int UsuarioId { get; }
}
