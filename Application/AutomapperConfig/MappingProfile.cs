using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.AutomapperConfig;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Catálogos
        CreateMap<Marca, MarcaDto>();
        CreateMap<MarcaSaveDto, Marca>();
        CreateMap<Modelo, ModeloDto>();
        CreateMap<ModeloSaveDto, Modelo>();
        CreateMap<Servicio, ServicioDto>();
        CreateMap<ServicioSaveDto, Servicio>();
        CreateMap<EstadoOrden, EstadoOrdenDto>();
        CreateMap<EstadoOrdenSaveDto, EstadoOrden>();
        CreateMap<ProblemaReportado, ProblemaReportadoDto>();
        CreateMap<ProblemaReportadoSaveDto, ProblemaReportado>();

        // Clientes y vehículos
        CreateMap<Cliente, ClienteDto>();
        CreateMap<ClienteSaveDto, Cliente>();
        CreateMap<Vehiculo, VehiculoDto>();
        CreateMap<VehiculoSaveDto, Vehiculo>();

        // Órdenes: el Total lo calcula la vista SQL y lo asigna el servicio.
        CreateMap<DetalleOrdenTrabajo, DetalleOrdenTrabajoDto>();
        CreateMap<OrdenTrabajo, OrdenTrabajoDto>()
            .ForCtorParam(nameof(OrdenTrabajoDto.Total), o => o.MapFrom(_ => 0m));

        // Usuarios
        CreateMap<Usuario, UsuarioDto>()
            .ForCtorParam(nameof(UsuarioDto.Rol), o => o.MapFrom(u => u.Rol.Nombre));
    }
}
