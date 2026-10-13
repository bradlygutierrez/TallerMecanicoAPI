namespace Domain.Entities;

/// <summary>Ids generados por el script de arranque (IDENTITY empieza en 1, en el orden de inserción).</summary>
public static class EstadoExistenciaIds
{
    public const int Activo = 1;
    public const int Inactivo = 2;
}

public static class EstadoOrdenIds
{
    public const int Pendiente = 1;
    public const int EnProceso = 2;
    public const int Completada = 3;
    public const int Cancelada = 4;
}

public static class UsuarioIds
{
    /// <summary>Usuario 'sistema': actor de auditoría cuando no hay un usuario autenticado.</summary>
    public const int Sistema = 1;
}

public static class RolesNombres
{
    public const string Administrador = "Administrador";
}
