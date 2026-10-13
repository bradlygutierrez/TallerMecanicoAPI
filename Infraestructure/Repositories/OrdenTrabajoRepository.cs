using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class OrdenTrabajoRepository(TallerDbContext contexto)
    : Repository<OrdenTrabajo>(contexto), IOrdenTrabajoRepository
{
    public Task<OrdenTrabajo?> ObtenerConDetallesAsync(int id, CancellationToken ct = default) =>
        Set.Include(o => o.Detalles).FirstOrDefaultAsync(o => o.Id == id, ct);

    public Task<List<OrdenTrabajo>> BuscarAsync(int? vehiculoId, int? estadoOrdenId, CancellationToken ct = default)
    {
        var consulta = Set.AsNoTracking();
        if (vehiculoId.HasValue) consulta = consulta.Where(o => o.VehiculoId == vehiculoId.Value);
        if (estadoOrdenId.HasValue) consulta = consulta.Where(o => o.EstadoOrdenId == estadoOrdenId.Value);
        return consulta.OrderByDescending(o => o.FechaIngreso).ToListAsync(ct);
    }

    public async Task<Dictionary<int, decimal>> ObtenerTotalesAsync(IEnumerable<int> ordenIds, CancellationToken ct = default)
    {
        var ids = ordenIds.ToList();
        return await Contexto.TotalesOrden
            .Where(t => ids.Contains(t.OrdenTrabajoId))
            .ToDictionaryAsync(t => t.OrdenTrabajoId, t => t.Total, ct);
    }
}
