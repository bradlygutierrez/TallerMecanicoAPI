using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TallerMecanicoAPI.Controllers;

[ApiController]
[Authorize]
public abstract class CrudControllerBase<TDto, TSaveDto>(ICrudService<TDto, TSaveDto> servicio)
    : ControllerBase where TDto : IConId
{
    [HttpGet]
    public async Task<ActionResult<List<TDto>>> Listar(CancellationToken ct) =>
        Ok(await servicio.ListarAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TDto>> Obtener(int id, CancellationToken ct)
    {
        var resultado = await servicio.ObtenerAsync(id, ct);
        return resultado is null ? NotFound() : Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<TDto>> Crear([FromBody] TSaveDto dto, CancellationToken ct)
    {
        var creado = await servicio.CrearAsync(dto, ct);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TDto>> Actualizar(int id, [FromBody] TSaveDto dto, CancellationToken ct)
    {
        var resultado = await servicio.ActualizarAsync(id, dto, ct);
        return resultado is null ? NotFound() : Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct) =>
        await servicio.EliminarAsync(id, ct) ? NoContent() : NotFound();
}
