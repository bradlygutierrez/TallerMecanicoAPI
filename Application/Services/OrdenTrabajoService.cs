using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public interface IOrdenTrabajoService
{
    Task<List<OrdenTrabajoDto>> ListarAsync(int? vehiculoId, int? estadoOrdenId, CancellationToken ct = default);
    Task<OrdenTrabajoDto?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<OrdenTrabajoDto> CrearAsync(OrdenTrabajoCrearDto dto, CancellationToken ct = default);
    Task<OrdenTrabajoDto?> CambiarEstadoAsync(int id, CambiarEstadoOrdenDto dto, CancellationToken ct = default);
}

public class OrdenTrabajoService(
    IOrdenTrabajoRepository ordenes,
    IRepository<Vehiculo> vehiculos,
    IRepository<ProblemaReportado> problemas,
    IRepository<EstadoOrden> estados,
    IRepository<Servicio> servicios,
    IUnitOfWork unidadTrabajo,
    IMapper mapper) : IOrdenTrabajoService
{
    public async Task<List<OrdenTrabajoDto>> ListarAsync(int? vehiculoId, int? estadoOrdenId, CancellationToken ct = default)
    {
        var lista = await ordenes.BuscarAsync(vehiculoId, estadoOrdenId, ct);
        var totales = await ordenes.ObtenerTotalesAsync(lista.Select(o => o.Id), ct);
        return lista
            .Select(o => mapper.Map<OrdenTrabajoDto>(o) with { Total = totales.GetValueOrDefault(o.Id) })
            .ToList();
    }

    public async Task<OrdenTrabajoDto?> ObtenerAsync(int id, CancellationToken ct = default)
    {
        var orden = await ordenes.ObtenerConDetallesAsync(id, ct);
        return orden is null ? null : await ConTotalAsync(orden, ct);
    }

    public async Task<OrdenTrabajoDto> CrearAsync(OrdenTrabajoCrearDto dto, CancellationToken ct = default)
    {
        if (await vehiculos.ObtenerPorIdAsync(dto.VehiculoId, ct) is null)
            throw new NegocioException("El vehículo no existe o está inactivo.");
        if (await problemas.ObtenerPorIdAsync(dto.ProblemaReportadoId, ct) is null)
            throw new NegocioException("El problema reportado no existe.");
        if (dto.Detalles.GroupBy(d => d.ServicioId).Any(g => g.Count() > 1))
            throw new NegocioException("No se puede repetir un servicio dentro de la misma orden.");

        var orden = new OrdenTrabajo
        {
            VehiculoId = dto.VehiculoId,
            ProblemaReportadoId = dto.ProblemaReportadoId,
            EstadoOrdenId = EstadoOrdenIds.Pendiente,
            EstadoExistenciaId = EstadoExistenciaIds.Activo,
            FechaIngreso = DateTime.UtcNow
        };

        foreach (var d in dto.Detalles)
        {
            var servicio = await servicios.ObtenerPorIdAsync(d.ServicioId, ct)
                ?? throw new NegocioException($"El servicio {d.ServicioId} no existe o está inactivo.");

            orden.Detalles.Add(new DetalleOrdenTrabajo
            {
                ServicioId = servicio.Id,
                Cantidad = d.Cantidad,
                PrecioUnitarioAplicado = d.PrecioUnitarioAplicado ?? servicio.PrecioBase,
                EstadoExistenciaId = EstadoExistenciaIds.Activo
            });
        }

        await ordenes.AgregarAsync(orden, ct);
        await unidadTrabajo.SaveChangesAsync(ct);
        return await ConTotalAsync(orden, ct);
    }

    public async Task<OrdenTrabajoDto?> CambiarEstadoAsync(int id, CambiarEstadoOrdenDto dto, CancellationToken ct = default)
    {
        var orden = await ordenes.ObtenerConDetallesAsync(id, ct);
        if (orden is null) return null;

        if (await estados.ObtenerPorIdAsync(dto.EstadoOrdenId, ct) is null)
            throw new NegocioException("El estado de orden no existe.");
        if (orden.EstadoOrdenId is EstadoOrdenIds.Completada or EstadoOrdenIds.Cancelada)
            throw new NegocioException("Una orden completada o cancelada no puede cambiar de estado.");

        orden.EstadoOrdenId = dto.EstadoOrdenId;
        await unidadTrabajo.SaveChangesAsync(ct);
        return await ConTotalAsync(orden, ct);
    }

    private async Task<OrdenTrabajoDto> ConTotalAsync(OrdenTrabajo orden, CancellationToken ct)
    {
        var totales = await ordenes.ObtenerTotalesAsync([orden.Id], ct);
        return mapper.Map<OrdenTrabajoDto>(orden) with { Total = totales.GetValueOrDefault(orden.Id) };
    }
}
