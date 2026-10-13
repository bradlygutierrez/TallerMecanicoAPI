using Domain.Entities;

namespace Domain.Interfaces;

public interface IRepository<T> where T : Entidad
{
    /// <summary>Devuelve la entidad con seguimiento (tracked) respetando el borrado lógico.</summary>
    Task<T?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<List<T>> ListarAsync(CancellationToken ct = default);
    IQueryable<T> Consultar();
    Task AgregarAsync(T entidad, CancellationToken ct = default);
    void Eliminar(T entidad);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IOrdenTrabajoRepository : IRepository<OrdenTrabajo>
{
    Task<OrdenTrabajo?> ObtenerConDetallesAsync(int id, CancellationToken ct = default);
    Task<List<OrdenTrabajo>> BuscarAsync(int? vehiculoId, int? estadoOrdenId, CancellationToken ct = default);
    /// <summary>Totales calculados por la vista taller.vw_TotalOrdenTrabajo.</summary>
    Task<Dictionary<int, decimal>> ObtenerTotalesAsync(IEnumerable<int> ordenIds, CancellationToken ct = default);
}

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario, CancellationToken ct = default);
    /// <summary>Ignora el borrado lógico: el nombre es UNIQUE aunque el usuario esté inactivo.</summary>
    Task<bool> ExisteNombreAsync(string nombreUsuario, CancellationToken ct = default);
}
