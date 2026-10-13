using Application.AutomapperConfig;
using Application.Dtos;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddScoped<ICrudService<MarcaDto, MarcaSaveDto>, CrudService<Marca, MarcaDto, MarcaSaveDto>>();
        services.AddScoped<ICrudService<ModeloDto, ModeloSaveDto>, CrudService<Modelo, ModeloDto, ModeloSaveDto>>();
        services.AddScoped<ICrudService<ServicioDto, ServicioSaveDto>, CrudService<Servicio, ServicioDto, ServicioSaveDto>>();
        services.AddScoped<ICrudService<EstadoOrdenDto, EstadoOrdenSaveDto>, CrudService<EstadoOrden, EstadoOrdenDto, EstadoOrdenSaveDto>>();
        services.AddScoped<ICrudService<ProblemaReportadoDto, ProblemaReportadoSaveDto>, CrudService<ProblemaReportado, ProblemaReportadoDto, ProblemaReportadoSaveDto>>();
        services.AddScoped<ICrudService<ClienteDto, ClienteSaveDto>, CrudService<Cliente, ClienteDto, ClienteSaveDto>>();
        services.AddScoped<ICrudService<VehiculoDto, VehiculoSaveDto>, CrudService<Vehiculo, VehiculoDto, VehiculoSaveDto>>();

        services.AddScoped<IOrdenTrabajoService, OrdenTrabajoService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
