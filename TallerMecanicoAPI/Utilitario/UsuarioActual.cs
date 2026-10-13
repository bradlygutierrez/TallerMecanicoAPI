using Domain.Entities;
using Domain.Interfaces;

namespace TallerMecanicoAPI.Utilitario;

public class UsuarioActual(IHttpContextAccessor accesor) : IUsuarioActual
{
    public int UsuarioId =>
        int.TryParse(accesor.HttpContext?.User.FindFirst("sub")?.Value, out var id) ? id : UsuarioIds.Sistema;
}
