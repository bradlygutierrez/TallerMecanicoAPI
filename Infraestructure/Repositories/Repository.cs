using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class Repository<T>(TallerDbContext contexto) : IRepository<T> where T : Entidad
{
    protected readonly TallerDbContext Contexto = contexto;
    protected DbSet<T> Set => Contexto.Set<T>();

    public Task<T?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        Set.FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<List<T>> ListarAsync(CancellationToken ct = default) =>
        Set.AsNoTracking().ToListAsync(ct);

    public IQueryable<T> Consultar() => Set;

    public async Task AgregarAsync(T entidad, CancellationToken ct = default) =>
        await Set.AddAsync(entidad, ct);

    public void Eliminar(T entidad) => Set.Remove(entidad);
}
