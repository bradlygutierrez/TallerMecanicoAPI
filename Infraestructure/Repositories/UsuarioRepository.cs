using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class UsuarioRepository(TallerDbContext contexto) : Repository<Usuario>(contexto), IUsuarioRepository
{
    public Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario, CancellationToken ct = default) =>
        Set.Include(u => u.Rol).FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario, ct);

    public Task<bool> ExisteNombreAsync(string nombreUsuario, CancellationToken ct = default) =>
        Set.IgnoreQueryFilters().AnyAsync(u => u.NombreUsuario == nombreUsuario, ct);
}
