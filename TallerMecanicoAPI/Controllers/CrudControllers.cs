using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace TallerMecanicoAPI.Controllers;

[Route("api/marcas")]
public class MarcasController(ICrudService<MarcaDto, MarcaSaveDto> servicio)
    : CrudControllerBase<MarcaDto, MarcaSaveDto>(servicio);

[Route("api/modelos")]
public class ModelosController(ICrudService<ModeloDto, ModeloSaveDto> servicio)
    : CrudControllerBase<ModeloDto, ModeloSaveDto>(servicio);

[Route("api/servicios")]
public class ServiciosController(ICrudService<ServicioDto, ServicioSaveDto> servicio)
    : CrudControllerBase<ServicioDto, ServicioSaveDto>(servicio);

[Route("api/estados-orden")]
public class EstadosOrdenController(ICrudService<EstadoOrdenDto, EstadoOrdenSaveDto> servicio)
    : CrudControllerBase<EstadoOrdenDto, EstadoOrdenSaveDto>(servicio);

[Route("api/problemas-reportados")]
public class ProblemasReportadosController(ICrudService<ProblemaReportadoDto, ProblemaReportadoSaveDto> servicio)
    : CrudControllerBase<ProblemaReportadoDto, ProblemaReportadoSaveDto>(servicio);

[Route("api/clientes")]
public class ClientesController(ICrudService<ClienteDto, ClienteSaveDto> servicio)
    : CrudControllerBase<ClienteDto, ClienteSaveDto>(servicio);

[Route("api/vehiculos")]
public class VehiculosController(ICrudService<VehiculoDto, VehiculoSaveDto> servicio)
    : CrudControllerBase<VehiculoDto, VehiculoSaveDto>(servicio);
