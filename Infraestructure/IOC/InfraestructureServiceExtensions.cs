using Domain.Interfaces;
using Infraestructure.Database;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.IOC;

public static class InfraestructureServiceExtensions
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TallerDbContext>(opciones => opciones.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TallerDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IOrdenTrabajoRepository, OrdenTrabajoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        return services;
    }
}
