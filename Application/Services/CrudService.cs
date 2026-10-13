using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public interface ICrudService<TDto, TSaveDto>
{
    Task<List<TDto>> ListarAsync(CancellationToken ct = default);
    Task<TDto?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<TDto> CrearAsync(TSaveDto dto, CancellationToken ct = default);
    Task<TDto?> ActualizarAsync(int id, TSaveDto dto, CancellationToken ct = default);
    Task<bool> EliminarAsync(int id, CancellationToken ct = default);
}

/// <summary>
/// CRUD genérico. Las entidades con EstadoExistenciaId se desactivan (borrado lógico);
/// las demás se eliminan físicamente.
/// </summary>
public class CrudService<TEntity, TDto, TSaveDto>(IRepository<TEntity> repositorio, IUnitOfWork unidadTrabajo, IMapper mapper)
    : ICrudService<TDto, TSaveDto> where TEntity : Entidad
{
    public async Task<List<TDto>> ListarAsync(CancellationToken ct = default) =>
        mapper.Map<List<TDto>>(await repositorio.ListarAsync(ct));

    public async Task<TDto?> ObtenerAsync(int id, CancellationToken ct = default)
    {
        var entidad = await repositorio.ObtenerPorIdAsync(id, ct);
        return entidad is null ? default : mapper.Map<TDto>(entidad);
    }

    public async Task<TDto> CrearAsync(TSaveDto dto, CancellationToken ct = default)
    {
        var entidad = mapper.Map<TEntity>(dto);
        if (entidad is IEntidadConEstado conEstado)
            conEstado.EstadoExistenciaId = EstadoExistenciaIds.Activo;

        await repositorio.AgregarAsync(entidad, ct);
        await unidadTrabajo.SaveChangesAsync(ct);
        return mapper.Map<TDto>(entidad);
    }

    public async Task<TDto?> ActualizarAsync(int id, TSaveDto dto, CancellationToken ct = default)
    {
        var entidad = await repositorio.ObtenerPorIdAsync(id, ct);
        if (entidad is null) return default;

        mapper.Map(dto, entidad);
        await unidadTrabajo.SaveChangesAsync(ct);
        return mapper.Map<TDto>(entidad);
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
    {
        var entidad = await repositorio.ObtenerPorIdAsync(id, ct);
        if (entidad is null) return false;

        if (entidad is IEntidadConEstado conEstado)
            conEstado.EstadoExistenciaId = EstadoExistenciaIds.Inactivo;
        else
            repositorio.Eliminar(entidad);

        await unidadTrabajo.SaveChangesAsync(ct);
        return true;
    }
}
