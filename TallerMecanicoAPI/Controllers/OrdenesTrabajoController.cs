using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TallerMecanicoAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/ordenes-trabajo")]
public class OrdenesTrabajoController(IOrdenTrabajoService servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<OrdenTrabajoDto>>> Listar(
        [FromQuery] int? vehiculoId, [FromQuery] int? estadoOrdenId, CancellationToken ct) =>
        Ok(await servicio.ListarAsync(vehiculoId, estadoOrdenId, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdenTrabajoDto>> Obtener(int id, CancellationToken ct)
    {
        var orden = await servicio.ObtenerAsync(id, ct);
        return orden is null ? NotFound() : Ok(orden);
    }

    [HttpPost]
    public async Task<ActionResult<OrdenTrabajoDto>> Crear([FromBody] OrdenTrabajoCrearDto dto, CancellationToken ct)
    {
        var orden = await servicio.CrearAsync(dto, ct);
        return CreatedAtAction(nameof(Obtener), new { id = orden.Id }, orden);
    }

    [HttpPut("{id:int}/estado")]
    public async Task<ActionResult<OrdenTrabajoDto>> CambiarEstado(
        int id, [FromBody] CambiarEstadoOrdenDto dto, CancellationToken ct)
    {
        var orden = await servicio.CambiarEstadoAsync(id, dto, ct);
        return orden is null ? NotFound() : Ok(orden);
    }
}
