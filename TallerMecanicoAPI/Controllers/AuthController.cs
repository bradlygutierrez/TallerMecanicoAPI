using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TallerMecanicoAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService servicio) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginRespuestaDto>> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var respuesta = await servicio.LoginAsync(dto, ct);
        return respuesta is null ? Unauthorized(new ProblemDetails { Title = "Credenciales inválidas." }) : Ok(respuesta);
    }

    [Authorize(Roles = RolesNombres.Administrador)]
    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioDto>> Registrar([FromBody] RegistrarUsuarioDto dto, CancellationToken ct) =>
        Ok(await servicio.RegistrarAsync(dto, ct));
}
