using Application.Services;
using Domain.Entities;
using Infraestructure.Database;
using Microsoft.EntityFrameworkCore;

namespace TallerMecanicoAPI.Utilitario;

public static class DbInicializador
{
    /// <summary>
    /// Crea el primer administrador si Seed:Admin está configurado y el usuario no existe.
    /// El script SQL solo crea el usuario 'sistema', que no tiene contraseña.
    /// </summary>
    public static async Task CrearAdministradorAsync(IServiceProvider servicios, IConfiguration configuracion)
    {
        var nombre = configuracion["Seed:Admin:NombreUsuario"];
        var password = configuracion["Seed:Admin:Password"];
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(password)) return;

        using var alcance = servicios.CreateScope();
        var db = alcance.ServiceProvider.GetRequiredService<TallerDbContext>();
        var hasher = alcance.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if (await db.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.NombreUsuario == nombre)) return;

        var rol = await db.Roles.FirstAsync(r => r.Nombre == RolesNombres.Administrador);
        db.Usuarios.Add(new Usuario
        {
            RolId = rol.Id,
            NombreUsuario = nombre,
            PasswordHash = hasher.Hashear(password),
            EstadoExistenciaId = EstadoExistenciaIds.Activo
        });
        await db.SaveChangesAsync();
    }
}
